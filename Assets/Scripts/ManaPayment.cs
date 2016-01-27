using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaPayment : MonoBehaviour {

	private int totalCost;
	private List<string> colours;

	void Awake(){
		colours = new List<string> ();
	}

	public void SetCost(string tileColour, string[] objectiveColour){
		colours.Clear();
		colours.Add (tileColour);
		colours.AddRange (objectiveColour);
		totalCost = colours.Count + 1;
	}

	public void CheckCost(){
	}
}
