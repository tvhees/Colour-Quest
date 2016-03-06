using UnityEngine;
using System.Collections;

public class ObjectivePool : ObjectPool {

	public GameObject objCube;
	public Material[] baseMaterials, nullMaterials;
	public Hand hand;
	public Discard discard;
	public Deck deck;
	public Objectives objectives;

	private int total = 0, threshold = 1;

	// Use this for initialization
	void Start () {
		CreatePool (40, objCube);
		NewTracker();
	}

	void NewTracker(){
		objectives.Clear ();

		threshold++;
		total = 0;

		for (int i = 0; i < threshold; i++) {
			GameObject obj = GetObject ();
			obj.transform.SetParent (transform);
			obj.SetActive (true);
			obj.GetComponent<MeshRenderer> ().material = nullMaterials [0];
			objectives.AddMana (obj);
		}
	}

	public void UpdateTracker(int[] value){
		int sum = value.Sum();
		for(int i = 0; i < sum; i++){
			objectives.contents [total].GetComponent<MeshRenderer> ().material = baseMaterials[i];
			total++;
			if (total == threshold) {
				hand.IncreaseLimit(1);
				NewTracker ();
			}
		}
	}

}
