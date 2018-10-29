using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {
	public AudioClip[] fxs;

	private static AudioControl instance;


	public static bool PlayFX( string name, float volume = 1 ) {
		int _len;
		bool _resp;
		
		_resp = false;
		if ( instance != null ) {
			_len = instance.fxs.Length;
			for ( int _num = 0; _num < _len; _num++ ) {
				if ( !instance.fxs[_num].name.Equals(name) ) continue;

				_resp = true;
				AudioSource.PlayClipAtPoint( instance.fxs[_num], Vector3.zero, volume );
				break;
			}
		}

		return _resp;
	}
	
	public static void PlayFX( AudioClip clip, float volume = 1 ) {
		AudioSource.PlayClipAtPoint( clip, Vector3.zero, volume );
	}

	void Awake() {
		instance = this;
	}
}
