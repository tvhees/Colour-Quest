using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : ObjectPool {

    // Variables
    public GameObject manaSphere;
	public Material[] baseMaterials, advancedMaterials, nullMaterials, startMaterials;
    public Hand hand;
    public Discard discard;
    public Deck deck;

	private int materialIndex = 0;

    // Methods
    public void Reset() {
        if(pool == null)
            CreatePool(80, manaSphere);

        startMaterials.Randomise();


        for (int i = 0; i < hand.maxHandSize; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            RandomColour(mana);
            mana.SetActive(true);
            hand.SendToHand(mana);
        }

        for (int i = 0; i < hand.maxHandSize * 2; i++)
        {
            GameObject mana = GetObject();
            mana.transform.SetParent(transform);
            RandomColour(mana);
            mana.SetActive(true);
            deck.SendToDeck(mana);
        }
    }

    public void SendToPool(GameObject mana)
    {
        if (hand.selectedMana.Contains(mana))
        {
            hand.selectedMana.Remove(mana);
        }

        hand.Remove(mana);

        deck.Remove(mana);

        discard.Remove(mana);

        ReturnObject(mana);
    }

    private void RandomColour(GameObject mana){
		if (materialIndex == startMaterials.Length) {
			materialIndex = 0;
			startMaterials.Randomise();
		}

		mana.GetComponent<MeshRenderer> ().material = startMaterials[materialIndex];
        switch (startMaterials[materialIndex].name) {
            case "Blue":
                mana.GetComponent<Mana>().value = new int[3] { 1, 0, 0 };
                break;
            case "Red":
                mana.GetComponent<Mana>().value = new int[3] { 0, 1, 0 };
                break;
            case "Yellow":
                mana.GetComponent<Mana>().value = new int[3] { 0, 0, 1 };
                break;
        }

		mana.GetComponent<Mana> ().SaveState();

		materialIndex++;
	}

    private void SpecificColour(GameObject mana, int[] value) {
        mana.GetComponent<Mana>().value = value;

        Material material;

        if (value[0] == 1)
            if (value[1] == 1)
                material = advancedMaterials[0];
            else if (value[2] == 1)
                material = advancedMaterials[1];
            else material = baseMaterials[0];
        else if (value[1] == 1)
            if (value[2] == 1)
                material = advancedMaterials[2];
            else material = baseMaterials[1];
        else if (value[2] == 1)
            material = baseMaterials[2];
        else material = nullMaterials[0];

        mana.GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    public GameObject GetManaOption(int[] value, int blackMana) {
        GameObject mana = GetObject();

        int[] newValue = new int[3];
        for (int i = 0; i < value.Length; i++) {
            newValue[i] = value[(i + value.Length - blackMana) % value.Length];
        }

        SpecificColour(mana, newValue);

		mana.GetComponent<Mana> ().SaveState();

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
