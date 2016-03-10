using UnityEngine;
using System.Collections;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Discard discard;

	public void SendToDeck(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		AddMana (mana);
	}

	public void RefillDeck(int previewSize){
		if (contents.Count < previewSize) {
			while (discard.contents.Count > 0)
				SendToDeck (discard.contents [Random.Range (0, discard.contents.Count)]);
		}
	}

}
