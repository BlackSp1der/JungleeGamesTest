using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {
	private TextMesh[] labels;

	private static ScoreControl instance;


	public static void Set( int value ) {
		int _len;
		string _str;

		_str = value.ToString();
		_len = instance.labels.Length;
		for ( int _num = 0; _num < _len; _num++ ) {
			instance.labels[_num].text = _str;
		}
	}

	void Awake() {
		instance = this;
		labels = GetComponentsInChildren<TextMesh>();
	}
}
