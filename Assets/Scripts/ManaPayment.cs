using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaPayment : MonoBehaviour {

    public Player playerScript;
    public Hand hand;
    public ManaPool manaPool;
    public BoardScript boardScript;
	public GameObject selectionMarker;
	public ObjectivePool objectivePool;
    public ParticleSystem playerParticles;
    public bool payed = false;

	private int[] colourCost = new int[3], objectiveValue = new int[3], payment;
    private GameObject target;

    public void Reset()
    {
        Game.Instance.state = Game.State.IDLE;

        selectionMarker.transform.position = new Vector3(-10f, 10f, -10f);
        playerParticles.Stop();

        payed = false;
        payment = new int[3] { 0, 0, 0 };
        objectiveValue = new int[3] { 0, 0, 0 };
        target = null;

        for (int i = 0; i < hand.selectedMana.Count; i++)
        {
            hand.selectedMana[i].GetComponent<Mana>().selectFX.Stop();
        }

        hand.selectedMana.Clear();
    }

    public void SetCost(int[] tileColour, int[] objectiveColour, GameObject tile){
        Game.Instance.state = Game.State.PAYING;

        if (target != null)
            target.GetComponent<TileScript>().currentTile = false;
		
        colourCost = tileColour.Zip(objectiveColour);
		objectiveValue = objectiveColour;
        target = tile;
	}

	public void CheckPayment(int[] delta, bool add){
        if(add)
            payment = payment.Zip (delta, 1);
        else
            payment = payment.Zip(delta, 1, -1);

        int[] remainder = payment.Zip (colourCost, -1);

        // Check if all required costs have been payed, if so highlight the player object
        payed = true;
        for (int i = 0; i < remainder.Length; i++)
        {
            if (remainder[i] > 0)
            {
                payed = false;
                playerParticles.Stop();
            }
        }
        if (payed)
            playerParticles.Play();

    }

    public void ConfirmPayment() {
		StartCoroutine(playerScript.SmoothMovement(target.transform.parent.position, target.transform.parent.gameObject));

        if (objectiveValue.Sum() > 0)
			objectivePool.UpdateTracker (objectiveValue);

        GameObject manaReward = manaPool.GetObjectiveReward(objectiveValue);
        if(manaReward != null)
            hand.SendToHand(manaReward);

        hand.PaySelected();

        boardScript.FlipTiles(target.transform.parent.position);
        Reset();
    }
}
