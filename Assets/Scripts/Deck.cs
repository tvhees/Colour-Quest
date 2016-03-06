using UnityEngine;
using System.Collections;

public class Deck : ManaCollection<Deck> {
    
	public Hand hand;
	public Discard discard;

	public void SendToDeck(GameObject mana){
		hand.RemoveMana (mana);

		discard.RemoveMana (mana);

		AddMana (mana);
	}

	public void RefillDeck(int previewSize){
		if (contents.Count < previewSize) {
			while (discard.contents.Count > 0) {
				AddMana (discard.contents [Random.Range (0, contents.Count)]);
			}
		}
	}

}
