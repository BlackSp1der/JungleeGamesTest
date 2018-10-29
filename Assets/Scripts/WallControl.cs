using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {
	private Material mat;
	private int collisionCount = 3;
	private float blinkTimer;
	private float timer = 5;


	void Awake() {
		mat = GetComponent<MeshRenderer>().material;
	}

	void Update() {
		Color _col;

		_col = mat.color;
		if ( Time.time < blinkTimer ) {
			timer += Time.deltaTime;
			_col.a = Mathf.PingPong( timer * (3 - (blinkTimer - Time.time)), 1 );
			mat.color = _col;
		} else {
			_col.a = 1;
			mat.color = _col;
			if ( collisionCount < 1 ) Destroy( gameObject );
			enabled = false;
		}
	}

	void OnCollisionEnter( Collision collision ) {
		if ( !enabled && collision.collider.CompareTag("Player") ) {
			collisionCount--;
			blinkTimer = Time.time + 2;
			enabled = true;
			AudioControl.PlayFX( "buzz" );
		}
	}
}
