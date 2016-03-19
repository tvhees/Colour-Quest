using UnityEngine;
using System.Collections;

public class Container : ClickableObject {

	public GameObject display;

	#if UNITY_STANDALONE || UNITY_EDITOR
	private void OnMouseUp() {
		ReleaseAction();
	}
	#endif

	public override void ClickAction(){
		transform.parent.SendMessage ("SendToDisplay");
		display.SetActive (true);
	}

	public void ReleaseAction(){
		display.SetActive (false);
	}

}
