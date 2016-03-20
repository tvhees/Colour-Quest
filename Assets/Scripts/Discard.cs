using UnityEngine;
using System.Collections;

public class Discard : Collection<Discard> {

    public Hand hand;
    public Deck deck;
    public GameObject discardContainer;
	public DisplayPanel discardDisplay;

    public override void Reset()
    {
		//discardContainer.transform.localPosition = new Vector3 (Screen.width * 0.43f, 0f, 0f);

		SharedSetup ();
    }

    public void SendToDiscard(GameObject mana)
    {
        // Remove from other lists
        if (hand.selectedMana.Contains(mana))
            hand.selectedMana.Remove(mana);

        hand.Remove(mana);

        // Reset any colour change or particles
        mana.GetComponent<Mana>().Reset();

        AddObj(mana);
    }

	public void SendToDisplay(){
		discardDisplay.UpdateDisplay (manaList);
	}
}
