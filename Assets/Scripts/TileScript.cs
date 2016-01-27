using UnityEngine;
using System.Collections;

public class TileScript : ClickableObject {

	public int[] tileCost;
	public bool currentTile = false;

	private GameObject player;
	private PlayerScript playerScript;
	private GameObject manaHand;
	private ManaPayment manaPayment;

	void Awake(){
		player = GameObject.Find ("PlayerObject");
		playerScript = player.GetComponent<PlayerScript> ();
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
	}

	public override void MouseClick(){
		manaPayment.SetCost (tileCost, new int[3]{ 0, 0, 0 });

		StartCoroutine (playerScript.SmoothMovement (transform.parent.position, transform.parent.gameObject));
	}

}
