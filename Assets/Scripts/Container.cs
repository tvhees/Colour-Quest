using UnityEngine;
using System.Collections;

public class Container : ClickableObject {

	public GameObject display;
	public GameObject source;

	#if UNITY_STANDALONE// || UNITY_EDITOR
	private void OnMouseUp() {
		ReleaseAction();
	}
	#endif

	public override void ClickAction(){
		source.SendMessage ("SendToDisplay");
		display.SetActive (true);
	}

	public void ReleaseAction(){
		display.SetActive (false);
	}

}
