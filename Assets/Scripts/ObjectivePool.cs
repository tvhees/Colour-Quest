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

    public void Reset() {
		homePosition = transform.position;

        if(pool == null)
            CreatePool(40, objCube);

        objectives.Reset();
        total = 0;
        threshold = 1;
        NewTracker();
    }

	public override void SendToPool (GameObject obj){
		objectives.Remove (obj);

		ReturnObject (obj);
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
			StartCoroutine(objectives.AddObj (obj));
		}
	}

	public void UpdateTracker(int[] value){
		int sum = value.Sum();

        if (sum == 1 && value[1] == 1)
            sum++;

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
