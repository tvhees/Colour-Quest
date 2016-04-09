using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : ObjectPool {

    // Variables
    public GameObject manaSphere, deckContainer;
	public Material[] materials, startMaterials;
    public Hand hand;
	public Preview preview;
    public Discard discard;
    public Deck deck;

    // Methods
    public IEnumerator Reset() {
		homePosition = deckContainer.transform.position;

        if(pool == null)
            CreatePool(80, manaSphere);

        // Set up the deck and discard
        deck.moveTime = 0.0001f;
        for (int i = 0; i < Save.Instance.deck.Count; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            SetColour(mana, Save.Instance.deck[i]);
            mana.SetActive(true);
            yield return StartCoroutine(deck.SendToDeck(mana, true));
        }

        discard.moveTime = 0.0001f;
        for (int i = 0; i < Save.Instance.discard.Count; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            SetColour(mana, Save.Instance.discard[i]);
            mana.SetActive(true);
            yield return StartCoroutine(discard.SendToDiscard(mana, true));
        }

        deck.moveTime = deck.startMoveTime;
        discard.moveTime = discard.startMoveTime;

        // Set up the hand and preview
        for (int i = 0; i < Save.Instance.hand.Count; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            SetColour(mana, Save.Instance.hand[i]);
            mana.SetActive(true);
            yield return StartCoroutine(hand.SendToHand(mana, true));
        }

        for (int i = 0; i < Save.Instance.preview.Count; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            SetColour(mana, Save.Instance.preview[i]);
            mana.SetActive(true);
            yield return StartCoroutine(preview.SendToPreview(mana, true));
        }

        Save.Instance.SaveGame();
        Master.Instance.loading = false;
    }

    public override void SendToPool(GameObject mana)
    {
        if (hand.selectedMana.Contains(mana))
        {
            hand.selectedMana.Remove(mana);
        }

        hand.Remove(mana);

        deck.Remove(mana);

        discard.Remove(mana);

        preview.Remove(mana);

        ReturnObject(mana);
    }

    private void SetColour(GameObject mana, int colourIndex){

		mana.GetComponent<MeshRenderer> ().sharedMaterial = materials[colourIndex];
        Mana m = mana.GetComponent<Mana>();
        m.colourIndex = colourIndex;
        switch (colourIndex) {
            case 0:
                m.value = new int[3] { 1, 0, 0 };
                break;
            case 1:
                m.value = new int[3] { 0, 1, 0 };
                break;
            case 2:
                m.value = new int[3] { 0, 0, 1 };
                break;
            case 3:
                m.value = new int[3] { 1, 1, 0 };
                break;
            case 4:
                m.value = new int[3] { 1, 0, 1 };
                break;
            case 5:
                m.value = new int[3] { 0, 1, 1 };
                break;
            case 6:
                m.value = new int[3] { 0, 0, 0 };
                break;
        }

		mana.GetComponent<Mana> ().SaveState();
	}

    private void SpecificColour(GameObject mana, int[] value) {
        mana.GetComponent<Mana>().value = value;

        Material material;

        if (value[0] == 1)
            if (value[1] == 1)
                material = materials[3];
            else if (value[2] == 1)
                material = materials[4];
            else material = materials[0];
        else if (value[1] == 1)
            if (value[2] == 1)
                material = materials[5];
            else material = materials[1];
        else if (value[2] == 1)
            material = materials[2];
        else material = materials[6];

        mana.GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    public GameObject GetManaOption(int[] value, int blackMana) {
        GameObject mana = GetObject();

        int[] newValue = new int[3];
        for (int i = 0; i < value.Length; i++) {
			newValue[i] = value[(i + value.Sum() * (value.Length - blackMana)) % value.Length];
        }

        SpecificColour(mana, newValue);

		mana.GetComponent<Mana> ().SaveState();

		mana.SetActive(true);

        return mana;
    }

    public GameObject GetObjectiveReward(int[] value) {
        GameObject mana = GetObject();
        int[] newValue = new int[3];

        if (value[0] == 1)
            if (value[1] == 1)
                newValue = new int[3] { 0, 0, 1 };
            else if (value[2] == 1)
                newValue = new int[3] { 1, 1, 0 };
            else {
                ReturnObject(mana);
                return null;
                }
        else if (value[1] == 1)
            if (value[2] == 1)
                newValue = new int[3] { 1, 0, 1 };
            else {
                ReturnObject(mana);
                return null;
            }
        else if (value[2] == 1)
            newValue = new int[3] { 0, 1, 0 };
        else {
            ReturnObject(mana);
            return null;
        }

        SpecificColour(mana, newValue);

        mana.SetActive(true);
        mana.GetComponent<Mana>().SaveState();

        return mana;
    }
}
