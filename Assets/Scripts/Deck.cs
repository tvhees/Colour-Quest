using UnityEngine;
using System.Collections;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Discard discard;

	public void SendToDeck(GameObject mana){
		hand.RemoveMana (mana);

		discard.RemoveMana (mana);

		AddMana (mana);
	}

	public void RefillDeck(int previewSize){
		if (contents.Count < previewSize) {
			while (discard.contents.Count > 0)
				SendToDeck (discard.contents [Random.Range (0, discard.contents.Count)]);
		}
	}

}
