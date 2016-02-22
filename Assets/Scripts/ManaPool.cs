using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : ObjectPool {

    // Variables
    public GameObject manaSphere;
	public Camera uiCamera;
	public Material[] materials;

	private int materialIndex = 0;

    // Methods
	void Start () {
		CreatePool (40, manaSphere);
		materials.Randomise();


		for (int i = 0; i < HandManager.Instance.maxHandSize; i++) {
			GameObject mana = GetObject ();
			mana.transform.SetParent (transform);
            RandomColour(mana);
            mana.SetActive(true);
            HandManager.Instance.MoveToHand(mana);
		}
	}

	void RandomColour(GameObject mana){
		if (materialIndex == materials.Length) {
			materialIndex = 0;
			materials.Randomise();
		}

		mana.GetComponent<MeshRenderer> ().material = materials[materialIndex];
        switch (materials[materialIndex].name) {
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
		materialIndex++;
	}
}
