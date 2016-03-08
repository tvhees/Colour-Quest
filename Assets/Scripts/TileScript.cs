using UnityEngine;
using System.Collections;

public class TileScript : ClickableObject {

	public int[] tileCost;
	public bool currentTile = false;
    public Material nullMaterial, colouredMaterial;


	private GameObject manaHand, boardHolder, selectionMarker;
    private BoardScript boardScript;
	private ManaPayment manaPayment;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        boardHolder = GameObject.Find("BoardHolder");
        boardScript = boardHolder.GetComponent<BoardScript>();
        selectionMarker = GameObject.Find("SelectionMarker");

        boardScript.hiddenTiles.Add(gameObject);
        transform.parent.rotation = Quaternion.Euler(0f, 0f, -180f);
        GetComponent<MeshRenderer>().material = nullMaterial;
	}

	public override void OnMouseDown(){

        switch (Game.Instance.state) {
            case Game.State.IDLE:
                // Check if tile is adjacent
                if ((Player.Instance.transform.position - transform.parent.position).sqrMagnitude < Player.Instance.moveDistance)
                {
                    Game.Instance.state = Game.State.PAYING;
                    selectionMarker.transform.position = transform.position;
                    selectionMarker.SetActive(true);

                    // If there's an objective on the tile we add its cost
                    int[] objectiveCost = new int[3] { 0, 0, 0 };
                    Objective objective = transform.parent.GetComponentInChildren<Objective>();
                    if (objective != null)
                        objectiveCost = objective.cost;

                    manaPayment.SetCost(tileCost, objectiveCost, gameObject);
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

    public void Flip(Vector3 playerPosition, float distance) {
        if ((transform.parent.position - playerPosition).sqrMagnitude < distance)
        {
            transform.parent.rotation = Quaternion.identity;
            GetComponent<MeshRenderer>().material = colouredMaterial;
            boardScript.hiddenTiles.Remove(gameObject);
        }
    }

}
