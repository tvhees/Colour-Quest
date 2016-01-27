using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaPayment : MonoBehaviour {

	private int totalCost;
	private int[] colourCost = new int[3];
	private int[] payment;

	void Awake(){
		payment = new int[3]{0, 0, 0}; // Blue, Red, Yellow
	}

	public void SetCost(int[] tileColour, int[] objectiveColour){
		colourCost = tileColour.Zip (objectiveColour);
		Debug.Log (colourCost[0] + ", " + colourCost[1] + ", " + colourCost[2]);
	}

	public void CheckPayment(int[] delta){
		payment = payment.Zip (delta);
		int[] remainder = payment.Zip (colourCost, -1);
		Debug.Log (remainder[0] + ", " + remainder[1] + ", " + remainder[2]);
	}
}
