using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : Collection<Hand> {

    public Deck deck;
    public Discard discard;
	public Goal goalScript;
    public List<GameObject> selectedMana, blackMana;
    public int maxHandSize, startHandSize = 5;

	private bool scrub;

    public override void Reset() {
        scrub = true;
        maxHandSize = startHandSize;

        while (contents.Count > 0) {
            manaPool.SendToPool(contents[0]);
        }
    }

    public void SendToHand(GameObject mana) {
        // Remove from discard pile
        discard.Remove(mana);

        // Remove from deck
        deck.Remove(mana);

        AddMana(mana);
    }

    public void PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            discard.SendToDiscard(selectedMana[0]);
        }

		scrub = false;

        // Draw new mana if hand is now empty
		for (int i = 0; i < contents.Count; i++) {
			if (!blackMana.Contains (contents [i]))
				return;
		}
            
		RefillHand();
    }

    public void RefillHand() {
        // Take mana from deck until hand is at current mana limit
        while (size < maxHandSize) {
            SendToHand(deck.contents[0]);
        }
		deck.RefillDeck(maxHandSize);

		scrub = true;

		StartCoroutine(goalScript.MoveGoal ());
    }

	public void ScrubHand(){
        // Gets rid of an entire hand, triggering refill from deck.

        // Clear any current selections or unnecessary black mana additions
        while (selectedMana.Count > 0) {
            selectedMana[0].GetComponent<Mana>().Select(true);
        }

        // Add mana to discard to selected list
		if(scrub)
			// Add everything
			selectedMana.AddRange (contents);
		else
			// Add only coloured mana
			for (int i = 0; i < contents.Count; i++) {
				if (!blackMana.Contains (contents [i]))
					selectedMana.Add (contents [i]);
			}
			
			
		PaySelected ();
	}

	public void IncreaseLimit(int increase){
		maxHandSize += increase;
		deck.RefillDeck (maxHandSize);
	}
}
