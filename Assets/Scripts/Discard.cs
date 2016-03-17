using UnityEngine;
using System.Collections;

public class Discard : Collection<Discard> {

    public Hand hand;
    public Deck deck;
    public GameObject discardContainer;

    public override void Reset()
    {
        discardContainer.transform.localPosition = new Vector2(Screen.width * 0.45f - discardContainer.transform.localScale.x * 0.6f, 0f);

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
