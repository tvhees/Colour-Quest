using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : Singleton<HandManager> {

    public Camera uiCamera;
    public RectTransform rTransform;
    public List<GameObject> selectedMana, discardMana, handMana;
    public int maxHandSize = 5;
	public float horizGap, verticalPos;
    public int handSize = 0, discardSize = 0;

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
        Vector2 screenPoint = new Vector2((handSize + 0.75f) * horizGap, verticalPos);
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
            handMana.Remove(mana);
            handSize--;
        }

		// Reset any colour change or particles
		mana.GetComponent<ManaScript>().Reset();

        // Add to discard pile
        discardMana.Add(mana);

        // Assign world space position to the right of existing discarded mana
		Vector2 screenPoint = new Vector2((discardSize + 0.75f) * horizGap, verticalPos - horizGap);
        Vector3 localPoint = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, uiCamera, out localPoint);
        mana.transform.position = localPoint;

        // Track mana in discard pile
        discardSize++;
    }


    public void PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            SendToDiscard(selectedMana[0]);
        }

        // Draw new mana if hand is now empty
        if (handMana.Count < 1)
            RefillHand();
    }

    void RefillHand() {
        // Take mana from discard pile until hand is at current mana limit
        while (handSize < maxHandSize) {
            SendToHand(discardMana[0]);
        }
    }

}
