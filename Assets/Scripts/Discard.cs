﻿using UnityEngine;
using System.Collections;

public class Discard : Collection {

    public Hand hand;
	public Preview preview;
    public Deck deck;
    public GameObject container;
	public DisplayPanel display;

    public override void Reset()
    {
		float width = GetComponent<RectTransform> ().rect.width;

		container.transform.localPosition = startHomePos = new Vector3 (0.40f * width, 0f, 0f);

		SharedSetup ();
    }

    public IEnumerator SendToDiscard(GameObject mana)
    {
        // Remove from other lists
        if (hand.selectedMana.Contains(mana))
            hand.selectedMana.Remove(mana);

        hand.Remove(mana);

		deck.Remove (mana);

		preview.Remove (mana);

        // Reset any colour change or particles
        mana.GetComponent<Mana>().Reset();

		StartCoroutine(AddObj(mana));

        yield return new WaitForSeconds(moveTime/4f);
    }

	public void SendToDisplay(){
		display.UpdateDisplay (manaList);
	}
}
