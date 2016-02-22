using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : Singleton<HandManager> {

    public Camera mainCamera;
    public List<GameObject> selectedMana, discardMana, handMana;
    public int maxHandSize = 5;
    
    private RectTransform rTransform;
    private int handSize = 0, discardSize = 0;

    void Start() {
        rTransform = GetComponent<RectTransform>();
    }

    public void MoveToHand(GameObject mana) {
        // Remove from discard pile
        if (discardMana.Contains(mana))
        {
            discardMana.Remove(mana);
            discardSize--;
        }

        // Add to hand
        handMana.Add(mana);

        // Assign world space position to the right of existing hand mana
        Vector2 screenPoint = new Vector2((handSize + 0.75f) * 55f, 150f);
        Vector3 localPoint = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, mainCamera, out localPoint);
        mana.transform.position = localPoint;

        // Track mana in hand
        handSize++;
    }

    public void MoveToDiscard(GameObject mana) {
        // Remove from other lists
        if (selectedMana.Contains(mana))
            selectedMana.Remove(mana);
        if (handMana.Contains(mana))
        {
            handMana.Remove(mana);
            handSize--;
        }

        // Add to discard pile
        discardMana.Add(mana);

        // Assign world space position to the right of existing discarded mana
        Vector2 screenPoint = new Vector2((handSize + 0.75f) * 55f, 100f);
        Vector3 localPoint = new Vector3();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, mainCamera, out localPoint);
        mana.transform.position = localPoint;

        // Track mana in discard pile
        discardSize++;
    }


    public void PaySelected() {
        // Move spent mana to discard
        while(selectedMana.Count > 0)
        {
            MoveToDiscard(selectedMana[0]);
        }

        // Draw new mana if hand is now empty
        if (handMana.Count < 1)
            RefillHand();
    }

    void RefillHand() {
        // Take mana from discard pile until hand is at current mana limit
        while (handSize < maxHandSize) {
            MoveToHand(discardMana[0]);
        }
    }

}
