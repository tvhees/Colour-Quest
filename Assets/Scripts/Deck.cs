using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : Collection {
    
	public Hand hand;
	public Preview preview;

	public Discard discard;
    public GameObject container;
	public DisplayPanel display;


    public override void Reset()
    {
		float width = GetComponent<RectTransform> ().rect.width;

		container.transform.localPosition = startHomePos = new Vector3 (-0.40f * width, 0f, 0f);

		SharedSetup ();
    }

    public IEnumerator SendToDeck(GameObject mana, bool setup = false) {
        if (!setup)
        {
            hand.Remove(mana);
            discard.Remove(mana);
            preview.Remove(mana);
            SaveSystem.Instance.deck.Add(mana.GetComponent<Mana>().colourIndex);
        }
        yield return StartCoroutine(AddObj(mana));
    }

    protected override void RemoveFromSave(int index)
    {
        SaveSystem.Instance.deck.RemoveAt(index);
    }

    public IEnumerator RefillDeck(){
        int i = 0;
		if (contents.Count < 1) {
            GameObject lastObject = null;

            while (discard.contents.Count > 0)
            {
                lastObject = discard.contents[Random.Range(0, discard.contents.Count)];
                StartCoroutine(SendToDeck(lastObject));
                yield return new WaitForSeconds(moveTime/4);
                i++;
                if (i > 50)
                {
                    Debug.Log("infinite loop: RefillDeck");
                    break;
                }
            }

            // Make sure we complete the refill before any other movement takes place
            // We look for the last object to be given a movement command and wait until
            // it has stopped moving.
            while (lastObject.GetComponent<ClickableObject>().moving) {
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

	public void SendToDisplay(){
		display.UpdateDisplay (manaList);
	}

}
