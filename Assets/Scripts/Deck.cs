using UnityEngine;
using System.Collections;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Discard discard;
    public GameObject deckContainer;
	public DisplayPanel deckDisplay;

    public override void Reset()
    {
		//deckContainer.transform.localPosition = new Vector3 (-Screen.width * 0.43f, 0f, 0f);

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
	}

	public void HideMana(){
		for (int i = 0; i < contents.Count; i++) {
			if (i < hand.maxHandSize)
				contents [i].transform.localScale = new Vector3 (objScale, objScale, objScale);
			else
				contents [i].transform.localScale = Vector3.zero;
		}
	}

	public void SendToDisplay(){
		deckDisplay.UpdateDisplay (manaList);
	}

}
