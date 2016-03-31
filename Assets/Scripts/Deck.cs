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

		container.transform.localPosition = startHomePos = new Vector3 (-0.40f * width, 0f, 0f);

		SharedSetup ();
    }

    public IEnumerator SendToDeck(GameObject mana){
		hand.Remove (mana);

		discard.Remove (mana);

		preview.Remove (mana);

		yield return StartCoroutine(AddObj(mana));
	}

	public IEnumerator RefillDeck(){
        int i = 0;
		if (contents.Count < 1) {
            float waitTime = discard.contents.Count * moveTime / 2;
            while (discard.contents.Count > 0)
            {
                StartCoroutine(SendToDeck(discard.contents[Random.Range(0, discard.contents.Count)]));
                yield return new WaitForSeconds(moveTime/4);
                i++;
                if (i > 50)
                {
                    Debug.Log("infinite loop: RefillDeck");
                    break;
                }
            }
            yield return new WaitForSeconds(waitTime);
		}
	}

	public void SendToDisplay(){
		display.UpdateDisplay (manaList);
	}

}
