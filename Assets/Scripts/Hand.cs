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
    public bool pause;

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
        GameObject lastObject = null;
        
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            lastObject = selectedMana[0];
            yield return StartCoroutine(discard.SendToDiscard(lastObject));
        }

        // Make sure we complete the payment before any other movement takes place
        // We look for the last object to be given a movement command and wait until
        // it has stopped moving.
        int loopbreaker = 0;
        while (lastObject.GetComponent<ClickableObject>().moving)
        {
            yield return new WaitForSeconds(moveTime);
            loopbreaker++;
            if (loopbreaker > 50)
            {
                Debug.Log("infinite loop: waiting");
                break;
            }
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
        GameObject lastObject = null;
        while (size < maxHandSize) {
            lastObject = preview.contents[0];
            yield return StartCoroutine(SendToHand(lastObject));
			i++;
			if (i > 50) {
				Debug.Log ("infinite loop: RefillHand");
				break;
			}
        }

        // Make sure we complete the refill before any other movement takes place
        // We look for the last object to be given a movement command and wait until
        // it has stopped moving.
        while (lastObject.GetComponent<ClickableObject>().moving)
        {
            yield return new WaitForSeconds(moveTime);
            i++;
            if (i > 100)
            {
                Debug.Log("infinite loop: waiting");
                break;
            }
        }

		yield return StartCoroutine(preview.RefillPreview(maxHandSize));

		scrub = true;

        Game.Instance.state = Game.State.GOAL;

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

	public IEnumerator IncreaseLimit(int increase){
		maxHandSize += increase;
		yield return StartCoroutine(preview.RefillPreview (maxHandSize));
	}
}
