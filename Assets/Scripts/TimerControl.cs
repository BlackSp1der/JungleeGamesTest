using UnityEngine;
using System.Collections;

public class TimerControl : MonoBehaviour {
	private TextMesh[] labels;

	private static TimerControl instance;


	public static void Set( int value ) {
		int _len;
		string _str;

		_str = value.ToString( "00" );
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
