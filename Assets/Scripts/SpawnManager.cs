using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[RequireComponent( typeof(MeshFilter) )]
public class SpawnManager : MonoBehaviour {
	private Transform trans;
	private MeshFilter meshFilter;
	private CubeData[] cubes;

	private static SpawnManager instance;
	private static List<CubeData> cubesData;

	private const string DATA_FILENAME	= "data";
	private const string COLOR_KEY		= "color";
	private const string POINTS_KEY		= "points";
	public const string CUBE_TAG		= "Cube";


	public static int RemainCubes() {
		int _count, _len;

		_count = 0;
		_len = instance.cubes.Length;
		for ( int _num = 0; _num < _len; _num++ ) {
			if ( instance.cubes[_num].objTrans != null ) _count++;
		}

		return _count;
	}

	public static CubeData CheckCollision( GameObject obj ) {
		Transform _trans;
		CubeData _resp;
		int _len;

		_resp = default(CubeData);
		_trans = obj.transform;
		_len = instance.cubes.Length;
		for ( int _num = 0; _num < _len; _num++ ) {
			if ( instance.cubes[_num].objTrans == null ) continue;
			if ( instance.cubes[_num].objTrans != _trans ) continue;

			_resp = instance.cubes[_num];
			instance.cubes[_num].clear();
			break;
		}

		return _resp;
	}

	void Awake() {
		instance = this;
		trans = transform;
		meshFilter = GetComponent<MeshFilter>();
		Random.seed = (int)System.DateTime.Now.Ticks;

		if ( Application.isPlaying ) {
			if ( cubesData == null ) loadData();

			cubes = new CubeData[10];
			for ( int _num = 0; _num < 10; _num++ ) {
				cubes[_num] = spawn();
			}
		}
	}

	CubeData spawn() {
		CubeData _cube;
		GameObject _obj;
		MeshRenderer _renderer;
		Vector3 _pos;
		int _len, _idx, _attempts;
		float _x, _y;
		bool _retry;

		_idx = Random.Range( 0, cubesData.Count );
		_obj = GameObject.CreatePrimitive( PrimitiveType.Cube );
		_obj.GetComponent<Collider>().isTrigger = true;
		_obj.transform.parent = trans.parent;
		
		_attempts = 20;
		_len = cubes.Length;
		do {
			_retry = false;
			_x = Random.Range( 0.1f, 0.5f ) * ( (Random.value > 0.5f)? 1 : -1 );
			_y = Random.Range( 0.1f, 0.5f ) * ( (Random.value > 0.5f)? 1 : -1 );
			_pos = new Vector3( _x, 0, _y );
			_pos.Scale( new Vector3(trans.lossyScale.x, 0, trans.lossyScale.y) );
			_pos += trans.position;
			_pos.y += 0.5f;
			for ( int _num = 0; _num < _len; _num++ ) {
				if ( cubes[_num].objTrans == null ) continue;
				if ( (cubes[_num].objTrans.position - _pos).sqrMagnitude > 10 ) continue;

				_retry = true;
				break;
			}
			_attempts--;
		} while ( _retry && (_attempts > 0) );

		_obj.transform.position = _pos - trans.position;
		_renderer = _obj.GetComponent<MeshRenderer>();
		_renderer.sharedMaterial = cubesData[_idx].mat;
		_obj.name = _idx.ToString();
		_obj.tag = CUBE_TAG;

		_cube = cubesData[_idx];
		_cube.objTrans = _obj.transform;

		return _cube;
	}

	void loadData() {
		TextAsset _data;
		string[] _lines, _str;
		string _key, _value;
		Hashtable _ht;
		int _len, _len2, _idx, _col, _pts;
		Color _color;

		cubesData = new List<CubeData>();
		_ht = new Hashtable();
		_data = Resources.Load<TextAsset>( DATA_FILENAME );
		_lines = _data.text.Split('\n');
		_len = _lines.Length;
		for ( int _num = 0; _num < _len; _num++ ) {
			_idx = _lines[_num].IndexOf("//");
			if ( _idx != -1 ) _lines[_num] = _lines[_num].Substring( 0, _idx );

			_str = _lines[_num].Split(',');
			_len2 = _str.Length;
			for ( int _num2 = 0; _num2 < _len2; _num2++ ) {
				_idx = _str[_num2].IndexOf(':');
				_key = _str[_num2].Substring( 0, _idx ).Trim().ToLower();
				_value = _str[_num2].Substring( _idx + 1 ).Trim();
				_ht.Add( _key, _value );
			}

			_color = Color.white;
			_value = (string)_ht[COLOR_KEY];
			if ( _value != null ) {
				if ( int.TryParse(_value, System.Globalization.NumberStyles.HexNumber, null, out _col) ) {
					_color.r = ( (_col >> 16) & 255 ) / 255;
					_color.g = ( (_col >> 8) & 255 ) / 255;
					_color.b = ( _col & 255 ) / 255;
				}
			}
			_pts = 0;
			_value = (string)_ht[POINTS_KEY];
			if ( _value != null ) {
				int.TryParse( _value, out _pts );
			}
			_ht.Clear();
			cubesData.Add( new CubeData(_color, _pts) );
		}
	}

	void OnDrawGizmos() {
		Vector3 _size;

		if ( Camera.current == Camera.main ) {
			_size = meshFilter.sharedMesh.bounds.size;
			_size.Scale( trans.lossyScale );
			Gizmos.color = Color.yellow * 0.4f;
			Gizmos.matrix = Matrix4x4.TRS( trans.position, trans.rotation, Vector3.one );
			Gizmos.DrawCube( trans.position, _size );
		}
	}
}

public struct CubeData {
	public Color color;
	public int points;
	public Material mat;
	public Transform objTrans;


	public CubeData( Color color, int points ) {
		this.color = color;
		this.points = points;
		this.objTrans = null;
		mat = new Material( Shader.Find("VertexLit") );
		mat.color = color;
	}

	public void clear() {
		points = 0;
		mat = null;
		objTrans = null;
	}
}