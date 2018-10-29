using UnityEngine;
using System.Collections;

public class TitleControl : MonoBehaviour {
	private TextMesh[] labels;

	private static TitleControl instance;

	public const string GAMEOVER_ANIM = "GameOver";
	public const string SUCCESS_ANIM = "Success";


	public static void Animate( string title, string animation ) {
		Set( title );
		instance.transform.localScale = Vector3.zero;
		instance.gameObject.SetActive( true );
		instance.animation.Play( animation );
	}

	public static void Set( string title ) {
		int _len;

		_len = instance.labels.Length;
		for ( int _num = 0; _num < _len; _num++ ) {
			instance.labels[_num].text = title;
		}
	}

	void Awake() {
		instance = this;
		labels = GetComponentsInChildren<TextMesh>();
		gameObject.SetActive( false );
	}
}
