using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preview : Collection<Preview> {
    
	public Hand hand;
	public Deck deck;
	public Discard discard;


    public override void Reset()
    {
		valueOnAdd = false;
		valueOnRemove = false;

		SharedSetup ();
    }

    public void SendToPreview(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		deck.Remove (mana);

		AddObj (mana);
	}

	public void RefillPreview(int nextHandSize){
		int i = 0;
		while (size < nextHandSize) {
			deck.RefillDeck();
			SendToPreview (deck.contents [Random.Range (0, deck.contents.Count)]);
			i++;
			if (i > 50) {
				Debug.Log ("infinite loop: RefillPreview");
				break;
			}
		}
	}
}
