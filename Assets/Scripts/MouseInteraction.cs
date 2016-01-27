using UnityEngine;
using System.Collections;

public class MouseInteraction : MonoBehaviour {

	public GameObject player;
	public bool currentTile = false;

	private PlayerScript playerScript;

	void Start(){
		player = GameObject.Find ("PlayerObject");
		playerScript = player.GetComponent<PlayerScript> ();
	}

	void OnMouseEnter(){
	}

	void OnMouseExit(){
	}

	void OnMouseUpAsButton(){
		StartCoroutine (playerScript.SmoothMovement (transform.parent.position, transform.parent.gameObject));
	}
}
