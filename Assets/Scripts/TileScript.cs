using UnityEngine;
using System.Collections;

public class TileScript : ClickableObject {

	public int[] tileCost;
	public bool currentTile = false;
    public Material nullMaterial, colouredMaterial, liveMaterial, deadMaterial;
    public float startRotation = -180f;

	private float rotationSpeed = 180f;
    private bool alive = true;
	private GameObject manaHand, boardHolder, selectionMarker;
    private BoardScript boardScript;
	private ManaPayment manaPayment;

	void Awake(){
		manaHand = GameObject.Find ("Hand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        boardHolder = GameObject.Find("BoardHolder");
        boardScript = boardHolder.GetComponent<BoardScript>();
        selectionMarker = GameObject.Find("SelectionMarker");

        boardScript.hiddenTiles.Add(gameObject);

        transform.parent.rotation = Quaternion.Euler(0f, 0f, startRotation);
        GetComponent<MeshRenderer>().material = nullMaterial;
	}

	public override void ClickAction(){

        if (alive)
        {
            switch (Game.Instance.state)
            {
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
                    if (currentTile)
                    {
                        Game.Instance.state = Game.State.IDLE;
                        currentTile = false;
                        manaPayment.Reset();
                        selectionMarker.SetActive(false);
                    }
                    break;
                case Game.State.GOAL:
                case Game.State.MENU:
                case Game.State.WON:
                case Game.State.LOST:
                    break;
            }
        }
	}

    public IEnumerator Flip(Vector3 playerPosition, float distance) {


        if ((transform.parent.position - playerPosition).sqrMagnitude < distance && alive)
        {
            int i = 0;
            float step = 0;
            while(step < 180f){
                step += rotationSpeed * Time.deltaTime;

				Debug.Log (step.ToString ());
                transform.parent.rotation = Quaternion.Euler(0f, 0f, startRotation + step);
                i++;
                if (i > 300)
                    break;
                yield return null;
            }

            transform.parent.rotation = Quaternion.identity;

            GetComponent<MeshRenderer>().material = colouredMaterial;
            boardScript.hiddenTiles.Remove(gameObject);
        }
    }

    public override void KillTile(bool dead)
    {

        if (transform.parent.childCount > 1)
            Destroy(transform.parent.GetChild(1).gameObject);

        if (dead)
        {
            alive = false;
            GetComponent<MeshRenderer>().material = deadMaterial;
        }
    }

}
