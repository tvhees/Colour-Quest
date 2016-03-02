using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : ObjectPool {

    // Variables
    public GameObject manaSphere;
	public Material[] baseMaterials, advancedMaterials, nullMaterials, startMaterials;

	private int materialIndex = 0;

    // Methods
	void Start () {
		CreatePool (40, manaSphere);
		startMaterials.Randomise();


		for (int i = 0; i < HandManager.Instance.maxHandSize; i++) {
			GameObject mana = GetObject ();
			mana.transform.SetParent (transform);
            RandomColour(mana);
            mana.SetActive(true);
            HandManager.Instance.SendToHand(mana);
		}

		HandManager.Instance.SetGap ();
	}

	private void RandomColour(GameObject mana){
		if (materialIndex == startMaterials.Length) {
			materialIndex = 0;
			startMaterials.Randomise();
		}

		mana.GetComponent<MeshRenderer> ().material = startMaterials[materialIndex];
        switch (startMaterials[materialIndex].name) {
            case "Blue":
                mana.GetComponent<ManaScript>().value = new int[3] { 1, 0, 0 };
                break;
            case "Red":
                mana.GetComponent<ManaScript>().value = new int[3] { 0, 1, 0 };
                break;
            case "Yellow":
                mana.GetComponent<ManaScript>().value = new int[3] { 0, 0, 1 };
                break;
        }

		mana.GetComponent<ManaScript> ().SaveState();

		materialIndex++;
	}

    private void SpecificColour(GameObject mana, int[] value) {
        mana.GetComponent<ManaScript>().value = value;

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

		mana.GetComponent<ManaScript> ().SaveState();

        return mana;
    }
}
