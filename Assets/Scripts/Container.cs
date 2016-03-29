using UnityEngine;
using System.Collections;

public class Container : ClickableObject {

	public GameObject display;
	public GameObject source;

	#if UNITY_STANDALONE || UNITY_EDITOR
	private void OnMouseUp() {
		// If the tutorial is running we want extra control over what happens
		if(Preferences.Instance.tutorial){
			if(Game.Instance.state == Game.State.IDLE || Game.Instance.state == Game.State.PAYING || Game.Instance.state == Game.State.GOAL){
				Game.Instance.tutorial.ClickAction(transform, "ReleaseAction");
			}
		}
		else
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
