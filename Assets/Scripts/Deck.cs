using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Preview preview;
	public Discard discard;
    public GameObject container;
	public DisplayPanel display;


    public override void Reset()
    {
		float width = GetComponent<RectTransform> ().rect.width;

		container.transform.localPosition = new Vector3 (-0.40f * width, 0f, 0f);

		SharedSetup ();
    }

    public void SendToDeck(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		preview.Remove (mana);

		AddObj (mana);
	}

	public void RefillDeck(){
		if (contents.Count < 1) {
			while (discard.contents.Count > 0)
				SendToDeck (discard.contents [Random.Range (0, discard.contents.Count)]);
		}
	}

	public void SendToDisplay(){
		display.UpdateDisplay (manaList);
	}

}
