using UnityEngine;
using System.Collections;

public class Discard : Collection<Discard> {

    public Hand hand;
    public Deck deck;

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
