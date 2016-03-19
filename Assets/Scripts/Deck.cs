using UnityEngine;
using System.Collections;

public class Deck : Collection<Deck> {
    
	public Hand hand;
	public Discard discard;
    public GameObject deckContainer;

    public override void Reset()
    {
		deckContainer.transform.localPosition = new Vector3 (-(Screen.width * 0.45f) + transform.localScale.x / 2f, 0f, 0f);

        while (contents.Count > 0)
        {
            manaPool.SendToPool(contents[0]);
        }
    }

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
