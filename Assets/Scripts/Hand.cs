using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Hand : Collection {

    public Game game;
    public Deck deck;
	public Preview preview;
    public Discard discard;
	public Goal goalScript;
    public List<GameObject> selectedMana;
    public bool pause;
    public TextMesh maxHandMesh;

	private bool scrub;

    public override void Reset() {
        Master.Instance.hand = this;
        scrub = true;
        SharedSetup ();
    }

    public IEnumerator SendToHand(GameObject mana, bool setup = false) {
        if (!setup)
        {
            // Remove from discard pile
            discard.Remove(mana);
            // Remove from deck
            deck.Remove(mana);
            // Remove from preview
            preview.Remove(mana);
            SaveSystem.Instance.hand.Add(mana.GetComponent<Mana>().colourIndex);
        }
        yield return StartCoroutine(AddObj(mana));
    }

    protected override void RemoveFromSave(int index)
    {
        SaveSystem.Instance.hand.RemoveAt(index);
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
        if (lastObject != null)
        {
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
        }

        scrub = false;

        // Draw new mana if hand is now empty
		for (int i = 0; i < contents.Count; i++) {
			if (!blackMana.Contains (contents [i]))
				yield break;
		}

        if (blackMana.Count >= SaveSystem.Instance.maxHandSize)
        {
            game.state = Game.State.LOST;
            Preferences.Instance.UpdateDifficulty(-1);
        }
        else
            yield return StartCoroutine(RefillHand());
    }

    public IEnumerator RefillHand() {
		int i = 0;
        // Take mana from deck until hand is at current mana limit
        GameObject lastObject = null;
        while (size < SaveSystem.Instance.maxHandSize) {
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

		yield return StartCoroutine(preview.RefillPreview(SaveSystem.Instance.maxHandSize));

		scrub = true;

        game.state = Game.State.GOAL;

		yield return StartCoroutine(goalScript.MoveGoal ());

        SaveSystem.Instance.SaveGame();
    }

	public void ScrubHand(){
        // Gets rid of an entire hand, triggering refill from deck.
        if (game.state == Game.State.IDLE || game.state == Game.State.PAYING)
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
		SaveSystem.Instance.maxHandSize += increase;
        SaveSystem.Instance.maxHandSize = SaveSystem.Instance.maxHandSize;
        maxHandMesh.text = SaveSystem.Instance.maxHandSize.ToString();
		yield return StartCoroutine(preview.RefillPreview (SaveSystem.Instance.maxHandSize));
        yield return new WaitForSeconds(0.5f);
	}
}
