using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Preview : Collection {
    
	public Hand hand;
	public Deck deck;
	public Discard discard;


    public override void Reset()
    {
		valueOnAdd = false;
		valueOnRemove = false;

		SharedSetup ();
    }

    public IEnumerator SendToPreview(GameObject mana, bool setup = false){
        if (!setup)
        {
            hand.Remove(mana);
            discard.Remove(mana);
            deck.Remove(mana);
            Save.Instance.preview.Add(mana.GetComponent<Mana>().colourIndex);
        }
        yield return StartCoroutine(AddObj(mana));
    }

    protected override void RemoveFromSave(int index)
    {
        Save.Instance.preview.RemoveAt(index);
    }

    public IEnumerator RefillPreview(int nextHandSize){
		int i = 0;
        GameObject lastObject = null;
		while (size < nextHandSize) {
			yield return StartCoroutine(deck.RefillDeck());
            lastObject = deck.contents[Random.Range(0, deck.contents.Count)];
            yield return StartCoroutine(SendToPreview (deck.contents [Random.Range (0, deck.contents.Count)]));
            i++;
			if (i > 50) {
				Debug.Log ("infinite loop: RefillPreview");
				break;
			}
		}

        // Make sure we complete the refill before any other movement takes place
        // We look for the last object to be given a movement command and wait until
        // it has stopped moving.
        while (lastObject.GetComponent<ClickableObject>().moving)
        {
            yield return new WaitForSeconds(moveTime);
            i++;
            if (i > 100)
            {
                Debug.Log("infinite loop: waiting");
                break;
            }
        }
    }
}
