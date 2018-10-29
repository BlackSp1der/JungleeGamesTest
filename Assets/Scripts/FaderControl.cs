using UnityEngine;
using System.Collections;

[RequireComponent( typeof(MeshRenderer) )]
public class FaderControl : MonoBehaviour {
	private Material mat;
	private int target;
	private float lerp;
	private float updateTimer;
	
	private static FaderControl instance;


	public static void FadeIn( float delay = 0 ) {
		instance.lerp = 0;
		instance.target = 0;
		instance.updateTimer = Time.time + delay;
		instance.enabled = true;
		instance.gameObject.SetActive( true );
	}

	public static void FadeOut( float delay = 0 ) {
		Color _color;

		instance.lerp = 0;
		instance.target = 1;
		instance.updateTimer = Time.time + delay;
		instance.enabled = true;
		instance.gameObject.SetActive( true );
		_color = instance.mat.color;
		_color.a = 1;
		instance.mat.color = _color;
	}

	void Awake() {
		instance = this;
		mat = GetComponent<MeshRenderer>().material;
		gameObject.SetActive( false );
		enabled = false;
	}

	void Update() {
		Color _col;

		if ( Time.time > updateTimer ) {
			lerp = Mathf.Lerp( lerp, 1.1f, Time.deltaTime );
			if ( lerp >= 1 ) {
				lerp = 1;
				enabled = false;
			}
			_col = mat.color;
			_col.a = ( target == 0 )? lerp : ( target - lerp );
			mat.color = _col;
		}
	}
}
