using UnityEngine;
using System.Collections;

public class Discard : Collection<Discard> {

    public Hand hand;
    public Deck deck;
    public GameObject container;
	public DisplayPanel display;

    public override void Reset()
    {
		float width = GetComponent<RectTransform> ().rect.width;

		container.transform.localPosition = new Vector3 (0.40f * width, 0f, 0f);

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
		display.UpdateDisplay (manaList);
	}
}
