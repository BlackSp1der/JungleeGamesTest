using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	private Transform trans;
	private bool die;


	void Awake() {
		trans = transform;
	}

	void Update() {
		if ( (GameManager.state == GameManager.PLAYING) && (trans.position.y < -1) ) {
			if ( !die ) AudioControl.PlayFX( "fall" );
			die = true;
			if ( trans.position.y < -13 ) GameManager.state = GameManager.DIE;
		}
	}

	void FixedUpdate() {
		float _x, _z;
		Vector3 _dir;

		switch ( GameManager.state ) {
			case GameManager.PLAYING:
				_dir = new Vector3();
				_dir.x = Input.acceleration.x;
				_dir.z = Input.acceleration.y;
				if ( _dir.sqrMagnitude == 0 ) {
					_dir.x = Input.GetAxis( "Horizontal" );
					_dir.z = Input.GetAxis( "Vertical" );
					_dir *= 3;
				} else {
					_dir *= 5;
				}
				rigidbody.AddForce( _dir * 200 * Time.fixedDeltaTime );
				break;
			default:
				if ( (GameManager.state != GameManager.STANDBY) && (rigidbody != null) ) {
					Destroy( rigidbody );
				}
				break;
		}
	}

	void OnTriggerEnter( Collider collider ) {
		CubeData _cube;

		if ( collider.CompareTag(SpawnManager.CUBE_TAG) ) {
			_cube = SpawnManager.CheckCollision( collider.gameObject );
			if ( _cube.objTrans != null ) {
				collider.enabled = false;
				GameManager.ConsumeCube( _cube );
				AudioControl.PlayFX( "cubecollision" );
			}
		}
	}
}
