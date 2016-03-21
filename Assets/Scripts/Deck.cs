using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Discard discard;
    public GameObject container;
	public DisplayPanel display;


    public override void Reset()
    {
		valueOnRemove = false;

		float width = GetComponent<RectTransform> ().rect.width;

		container.transform.localPosition = new Vector3 (-0.40f * width, 0f, 0f);

		SharedSetup ();
    }

    public void SendToDeck(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		AddObj (mana);
	}

	public void RefillDeck(int previewSize){
		if (contents.Count < previewSize) {
			while (discard.contents.Count > 0)
				SendToDeck (discard.contents [Random.Range (0, discard.contents.Count)]);
		}

		HideMana();
			
		float adjustment = contents [0].transform.localPosition.x;

		// Reposition objects slightly
		for (int i = 0; i < size; i++) {
			if (hand.maxHandSize % 2 < 1)
				adjustment /= 2;

			Debug.Log(contents [i].transform.localPosition);

			contents [i].transform.localPosition = contents [i].transform.localPosition - new Vector3 (adjustment, 0f, 0f);
		}
	}

	public void HideMana(){
		for (int i = 0; i < size; i++) {
			GameObject deckMana = contents [i];
			if (i < hand.maxHandSize) {
				if (hidden.Contains (deckMana))
					hidden.Remove (deckMana);

				if (!preview.Contains (deckMana)) {
					preview.Add (deckMana);
					deckMana.transform.localScale = new Vector3 (objScale, objScale, objScale);
					ChangeValue (deckMana, false);
				}
			} else {
				if (!hidden.Contains (deckMana))
					hidden.Add (deckMana);
					deckMana.transform.localScale = Vector3.zero;
			}
		}
	}

	public void SendToDisplay(){
		display.UpdateDisplay (manaList);
	}

}
