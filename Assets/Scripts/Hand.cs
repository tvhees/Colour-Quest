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
    public List<GameObject> selectedMana, blackMana;
    public int maxHandSize = 5;

    public void SendToHand(GameObject mana) {
        // Remove from discard pile
        discard.RemoveMana(mana);

        // Remove from deck
        deck.RemoveMana(mana);

        AddMana(mana);
    }

    public void PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            discard.SendToDiscard(selectedMana[0]);
        }

		scrubButton.interactable = false;

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
		scrubButton.interactable = true;
    }

	public void ScrubHand(){
        // Gets rid of an entire hand, triggering refill from deck.
		selectedMana.Clear();
		selectedMana.AddRange (contents);
		PaySelected ();
	}
}
