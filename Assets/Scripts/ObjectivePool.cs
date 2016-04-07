using UnityEngine;
using System.Collections;

public class ObjectivePool : ObjectPool {

	public GameObject objCube;
	public Material[] baseMaterials, nullMaterials;
	public Hand hand;
	public Discard discard;
	public Deck deck;
	public Objectives objectives;
    public Camera uiCamera, gameCamera;
    public ManaPool manaPool;

	private int total = 0, threshold = 1;

    public void Reset() {
		homePosition = transform.position;

        if(pool == null)
            CreatePool(40, objCube);

        objectives.Reset();
        total = 0;
        threshold = 1;
        StartCoroutine(NewTracker());
    }

	public override void SendToPool (GameObject obj){
		objectives.Remove (obj);

		ReturnObject (obj);
	}

	IEnumerator NewTracker(){
		objectives.Clear ();

		threshold++;
		total = 0;

        objectives.moveTime *= 4;

		for (int i = 0; i < threshold; i++) {
			GameObject obj = GetObject ();
			obj.transform.SetParent (transform);
			obj.SetActive (true);
			obj.GetComponent<MeshRenderer> ().material = nullMaterials [0];
			yield return StartCoroutine(objectives.AddObj (obj));
		}

        objectives.moveTime /= 4;
	}

	public IEnumerator UpdateTracker(int[] value, GameObject objectiveCube){
        // Find out what kind of objective it is
        int sum = value.Sum();
        if (sum == 1 && value[1] == 1)
            sum++;

        // Get the screen position of the objective
        Vector3 screenPos = gameCamera.WorldToScreenPoint(objectiveCube.transform.position);
        Vector3 worldPos = uiCamera.ScreenToWorldPoint(screenPos);

        // Send any colour rewards earned to the player's hand
        GameObject manaReward = manaPool.GetObjectiveReward(value);
        if (manaReward != null)
        {
            manaReward.transform.position = worldPos;
            yield return StartCoroutine(hand.SendToHand(manaReward));
        }

        bool cubeExists = true;
        Material objMaterial = objectiveCube.GetComponent<MeshRenderer>().material;

        for (int i = 0; i < sum; i++){
            if (cubeExists) {
                objectiveCube.transform.position = worldPos;
                objectiveCube.transform.SetParent(transform);
                yield return StartCoroutine(objectiveCube.GetComponent<ClickableObject>().SmoothMovement(objectives.contents[total].transform.position, objectives.moveTime, new Vector3 (objectives.objScale, objectives.objScale, objectives.objScale)));
                SendToPool(objectiveCube);
                cubeExists = false;
            }
			objectives.contents [total].GetComponent<MeshRenderer> ().material = objMaterial;
			total++;
			if (total == threshold) {
				yield return StartCoroutine(hand.IncreaseLimit(1));
				yield return NewTracker ();
			}
		}
	}

}
