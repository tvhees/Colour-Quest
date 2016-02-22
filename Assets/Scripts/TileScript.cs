using UnityEngine;
using System.Collections;

public class TileScript : ClickableObject {

	public int[] tileCost;
	public bool currentTile = false;

	private GameObject manaHand;
	private ManaPayment manaPayment;
    private GameObject selectionMarker;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        selectionMarker = GameObject.Find("SelectionMarker");
	}

	public override void MouseClick(){

        switch (Game.Instance.state) {
            case Game.State.IDLE:
                // Check if tile is adjacent
                if ((Player.Instance.transform.position - transform.parent.position).sqrMagnitude < Player.Instance.moveDistance)
                {
                    Game.Instance.state = Game.State.PAYING;
                    selectionMarker.transform.position = transform.position;
                    selectionMarker.SetActive(true);
                    manaPayment.SetCost(tileCost, new int[3] { 0, 0, 0 }, gameObject);
                    currentTile = true;
                }
                break;
            case Game.State.PAYING:
                if (currentTile) {
                    Game.Instance.state = Game.State.IDLE;
                    currentTile = false;
                    manaPayment.ResetCost();
                    selectionMarker.SetActive(false);
                }
                break;
            case Game.State.ENEMY:
                break;
        }
	}

}
