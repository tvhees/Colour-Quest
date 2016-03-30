﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : Collection<Hand> {

    public Deck deck;
	public Preview preview;
    public Discard discard;
	public Goal goalScript;
    public List<GameObject> selectedMana;
    public int maxHandSize, startHandSize = 5;

	private bool scrub;

    public override void Reset() {
        scrub = true;
        maxHandSize = startHandSize;
        SharedSetup ();
    }

    public IEnumerator SendToHand(GameObject mana) {
        // Remove from discard pile
        discard.Remove(mana);

        // Remove from deck
        deck.Remove(mana);

		// Remove from preview
		preview.Remove(mana);

		yield return StartCoroutine(AddObj(mana));
    }

    public IEnumerator PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            yield return StartCoroutine(discard.SendToDiscard(selectedMana[0]));
        }

		scrub = false;

        // Draw new mana if hand is now empty
		for (int i = 0; i < contents.Count; i++) {
			if (!blackMana.Contains (contents [i]))
				yield break;
		}
            
		yield return StartCoroutine(RefillHand());
    }

    public IEnumerator RefillHand() {
		int i = 0;
        // Take mana from deck until hand is at current mana limit
        while (size < maxHandSize) {
			yield return StartCoroutine(SendToHand(preview.contents[0]));
			i++;
			if (i > 50) {
				Debug.Log ("infinite loop: RefillHand");
				break;
			}

        }
		yield return StartCoroutine(preview.RefillPreview(maxHandSize));

		scrub = true;

		StartCoroutine(goalScript.MoveGoal ());
    }

	public void ScrubHand(){
        // Gets rid of an entire hand, triggering refill from deck.
        if (Game.Instance.state == Game.State.IDLE || Game.Instance.state == Game.State.PAYING)
        {
            // Clear any current selections or unnecessary black mana additions
            while (selectedMana.Count > 0)
            {
                selectedMana[0].GetComponent<Mana>().Select(true);
            }

            // Add mana to discard to selected list
            if (scrub)
                // Add everything
                selectedMana.AddRange(contents);
            else
                // Add only coloured mana
                for (int i = 0; i < contents.Count; i++)
                {
                    if (!blackMana.Contains(contents[i]))
                        selectedMana.Add(contents[i]);
                }


			StartCoroutine(PaySelected());
        }
	}

	public void IncreaseLimit(int increase){
		maxHandSize += increase;
		preview.RefillPreview (maxHandSize);
	}
}
