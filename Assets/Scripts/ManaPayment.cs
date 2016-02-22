using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaPayment : MonoBehaviour {

    public Player playerScript;

    private int totalCost;
	private int[] colourCost = new int[3];
	private int[] payment;
    private GameObject target;

	void Awake(){
		payment = new int[3]{ 0, 0, 0 }; // Blue, Red, Yellow
	}

	public void SetCost(int[] tileColour, int[] objectiveColour, GameObject tile){
        Game.Instance.state = Game.State.PAYING;

        if (target != null)
            target.GetComponent<TileScript>().currentTile = false;
        colourCost = tileColour.Zip(objectiveColour);
        target = tile;
	}

	public void CheckPayment(int[] delta, bool add){
        if(add)
            payment = payment.Zip (delta, 1);
        else
            payment = payment.Zip(delta, 1, -1);

        int[] remainder = payment.Zip (colourCost, -1);

        for (int i = 0; i < remainder.Length; i++) {
            if (remainder[i] > 0)
                return;
        }

        HandManager.Instance.PaySelected();

        StartCoroutine(playerScript.SmoothMovement(target.transform.parent.position, target.transform.parent.gameObject));

        ResetCost();
    }

    public void ResetCost() {
        Game.Instance.state = Game.State.IDLE;

        payment = new int[3] { 0, 0, 0 };
        target = null;
        HandManager.Instance.selectedMana.Clear();
    }
}
