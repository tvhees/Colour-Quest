using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hand : ManaCollection<Hand> {

	public Camera uiCamera;
    public Deck deck;
    public Discard discard;
    public Button scrubButton;
	public ManaPool manaPool;
    public List<GameObject> selectedMana, discardMana, handMana, blackMana;
    public int maxHandSize = 5;

    public Vector3 manaScale;

	private Vector3 worldGap;

    public void SendToHand(GameObject mana) {
        // Remove from discard pile
        discard.RemoveMana(mana);

        // Remove from deck
        deck.RemoveMana(mana);

        AddMana(mana);
    }

    public void SendToDiscard(GameObject mana) {
        // Remove from other lists
        if (selectedMana.Contains(mana))
            selectedMana.Remove(mana);
        if (handMana.Contains(mana))
        {
			int j = handMana.IndexOf (mana);
            handMana.Remove(mana);
			for (int i = j; i < handMana.Count; i++)
				handMana [i].transform.position = handMana [i].transform.position - worldGap;
            size--;
        }

		// Reset any colour change or particles
		mana.GetComponent<Mana>().Reset();

        discard.AddMana(mana);
    }

	public void SendToPool(GameObject mana){
		if (selectedMana.Contains (mana)) {
			selectedMana.Remove (mana);
		}

        RemoveMana(mana);

        deck.RemoveMana(mana);

        discard.RemoveMana(mana);

		if (blackMana.Contains (mana)) {
			blackMana.Remove (mana);
		}

		manaPool.ReturnObject (mana);
	}


    public void PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            SendToDiscard(selectedMana[0]);
        }

		scrubButton.interactable = false;

        // Draw new mana if hand is now empty
		for (int i = 0; i < handMana.Count; i++) {
			if (!blackMana.Contains (handMana [i]))
				return;
		}
            
		RefillHand();
    }

    public void RefillHand() {
        // Take mana from discard pile until hand is at current mana limit
        while (size < maxHandSize) {
            SendToHand(discardMana[0]);
        }

		scrubButton.interactable = true;
    }

	public void SetGap(){
		worldGap = handMana [1].transform.position - handMana [0].transform.position;
	}

	public void ScrubHand(){
		selectedMana.Clear();
		selectedMana.AddRange (handMana);
		PaySelected ();
	}
}
