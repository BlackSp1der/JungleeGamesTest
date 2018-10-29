using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {
	public const int STARTBUTTON = 1;


	void Start() {
	}

	void OnMouseUpAsButton() {
		SendMessageUpwards( "OnButton", STARTBUTTON, SendMessageOptions.DontRequireReceiver );
	}
}
