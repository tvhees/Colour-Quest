using UnityEngine;
using System.Collections;

public class MouseInteraction : MonoBehaviour {

	private ClickableObject objectScript;


	void Awake(){
		objectScript = GetComponent<ClickableObject> ();
	}

	void OnMouseUpAsButton(){
		objectScript.MouseClick();
		Debug.Log ("Clicked");
	}
}
