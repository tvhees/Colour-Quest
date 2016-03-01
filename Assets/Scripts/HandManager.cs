using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HandManager : Singleton<HandManager> {

	public Camera uiCamera;	
	public ManaPool manaPool;
    public RectTransform rTransform;
    public List<GameObject> selectedMana, discardMana, handMana, blackMana;
    public int maxHandSize = 5;
	public float horizGap, verticalPos;
	public Button scrubButton;
    
	private Vector3 worldGap;
	private int handSize = 0, discardSize = 0;

    public void SendToHand(GameObject mana) {
        // Remove from discard pile
        if (discardMana.Contains(mana))
        {
            discardMana.Remove(mana);
            discardSize--;
        }

        // Add to hand
        handMana.Add(mana);

        // Assign world space position to the right of existing hand mana
        Vector2 screenPoint = new Vector2((handSize + 1.0f) * horizGap, verticalPos);
        Vector3 localPoint = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, uiCamera, out localPoint);
        mana.transform.position = localPoint;
		mana.transform.parent = transform;
		mana.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

        // Track mana in hand
        handSize++;
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
            handSize--;
        }

		// Reset any colour change or particles
		mana.GetComponent<ManaScript>().Reset();

        // Add to discard pile
        discardMana.Add(mana);

        // Assign world space position to the right of existing discarded mana
		Vector2 screenPoint = new Vector2((discardSize + 1.0f) * horizGap, verticalPos - horizGap);
        Vector3 localPoint = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, uiCamera, out localPoint);
        mana.transform.position = localPoint;

        // Track mana in discard pile
        discardSize++;
    }

	public void SendToPool(GameObject mana){
		if (selectedMana.Contains (mana)) {
			selectedMana.Remove (mana);
		}

		if (handMana.Contains (mana)) {
			handMana.Remove (mana);
			handSize--;
		}

		if (discardMana.Contains (mana)) {
			discardMana.Remove (mana);
			discardSize--;
		}

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
        while (handSize < maxHandSize) {
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
