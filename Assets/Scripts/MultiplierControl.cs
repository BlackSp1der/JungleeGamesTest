using UnityEngine;
using System.Collections;

public class MultiplierControl : MonoBehaviour {
	public Camera GUICamera;

	private TextMesh label;
	private Transform trans;
	private float timer;
	private Color color;

	private static MultiplierControl instance;


	public static void ShowMultiplier( Color color, int value, Vector2 screenPos ) {
		Vector3 _pos;

		if ( instance.GUICamera != null ) {
			instance.color = color;
			instance.timer = Time.time + 1.5f;
			instance.label.color = color;
			instance.label.text = "x" + value;

			_pos = instance.GUICamera.ScreenToWorldPoint( screenPos );
			_pos.z = instance.trans.position.z;
			instance.trans.position = _pos;
			instance.enabled = true;
			instance.gameObject.SetActive( true );
		}
	}

	void Awake() {
		instance = this;
		trans = transform;
		label = GetComponent<TextMesh>();
	}

	void Update() {
		Vector3 _pos;
		float _lerp;

		_pos = trans.localPosition;
		_pos.y += Time.deltaTime;
		trans.localPosition = _pos;
		_lerp = Mathf.PingPong( 5 * Time.time, 1 );
		label.color = Color.Lerp( color, Color.white, _lerp );

		if ( Time.time > timer ) {
			enabled = false;
			gameObject.SetActive( false );
		}
	}
}
