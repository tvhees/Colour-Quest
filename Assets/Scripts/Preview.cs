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

    public IEnumerator SendToPreview(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		deck.Remove (mana);

		yield return StartCoroutine(AddObj(mana));
	}

	public IEnumerator RefillPreview(int nextHandSize){
		int i = 0;
		while (size < nextHandSize) {
			yield return StartCoroutine(deck.RefillDeck());
			yield return StartCoroutine(SendToPreview (deck.contents [Random.Range (0, deck.contents.Count)]));
            i++;
			if (i > 50) {
				Debug.Log ("infinite loop: RefillPreview");
				break;
			}
		}
	}
}
