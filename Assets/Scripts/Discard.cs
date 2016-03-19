using UnityEngine;
using System.Collections;

public class Discard : Collection<Discard> {

    public Hand hand;
    public Deck deck;
    public GameObject discardContainer;

    public override void Reset()
    {
		Debug.Log (Screen.width);
		discardContainer.transform.localPosition = new Vector3 ((Screen.width * 0.45f) - transform.localScale.x / 2f, 0f, 0f);

        while (contents.Count > 0)
        {
            manaPool.SendToPool(contents[0]);
        }
    }

    public void SendToDiscard(GameObject mana)
    {
        // Remove from other lists
        if (hand.selectedMana.Contains(mana))
            hand.selectedMana.Remove(mana);

        hand.Remove(mana);

        // Reset any colour change or particles
        mana.GetComponent<Mana>().Reset();

        AddMana(mana);
    }

}
