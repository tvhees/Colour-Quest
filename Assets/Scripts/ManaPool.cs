using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaPool : ObjectPool {

	public GameObject manaSphere;
	public int handSize;
	public Camera uiCamera;
	public Material[] materials;

	private List<GameObject> handList;
	private int materialIndex = 0;
	private RectTransform rTransform;


	// Use this for initialization
	void Start () {
		CreatePool (40, manaSphere);
		handList = new List<GameObject> ();
		materials.Randomise();
		rTransform = GetComponent<RectTransform> ();


		for (int i = 0; i < handSize; i++) {
			GameObject mana = GetObject ();
			handList.Add (mana);
			mana.transform.SetParent (transform);
			Vector2 screenPoint = new Vector2 ((i+0.75f)*35f, 100f);
			Vector3 localPoint = new Vector3();
			RectTransformUtility.ScreenPointToWorldPointInRectangle(rTransform, screenPoint, uiCamera, out localPoint);
			mana.transform.position = localPoint;
			RandomColour (mana);
			mana.SetActive (true);
		}
	}

	void RandomColour(GameObject mana){
		if (materialIndex == materials.Length) {
			materialIndex = 0;
			materials.Randomise();
		}

		mana.GetComponent<MeshRenderer> ().material = materials[materialIndex];
		materialIndex++;
	}
}
