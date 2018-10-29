using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {
	private bool busy;
	private float timer;


	void Start() {
		ScoreControl.Set( PlayerPrefs.GetInt(GameManager.BESTSCORE_KEY, 0) );
	}

	void Update() {
		if ( busy && (Time.time > timer) ) {
			Application.LoadLevel( "Game" );
			enabled = false;
		}
	}

	void OnButton( int button ) {
		if ( !busy ) {
			AudioControl.PlayFX( "button", 3 );
			FaderControl.FadeIn();
			timer = Time.time + 3;
			busy = true;
		}
	}
}
