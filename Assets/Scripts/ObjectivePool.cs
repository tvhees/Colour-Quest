using UnityEngine;
using System.Collections;

public class ObjectivePool : ObjectPool {

	public GameObject objCube;
	public Material[] materials;
	public Hand hand;
	public Discard discard;
	public Deck deck;
	public Objectives objectives;
    public Camera uiCamera, gameCamera;
    public ManaPool manaPool;

    public void Reset() {
		homePosition = transform.position;

        if(pool == null)
            CreatePool(40, objCube);

        objectives.Reset();
        StartCoroutine(NewTracker(false));
    }

	public override void SendToPool (GameObject obj){
		objectives.Remove (obj);

		ReturnObject (obj);
	}

	private IEnumerator NewTracker(bool resetTotal = true)
    {
        objectives.moveTime *= 4;

        if(resetTotal)
            Save.Instance.objectiveTracker[0] = 0;

		for (int i = 0; i < Save.Instance.objectiveTracker.Count - 1; i++) {
            GameObject obj;
            if (i < objectives.contents.Count)
                obj = objectives.contents[i];
            else
                obj = GetObject ();

			obj.transform.SetParent (transform);
			obj.SetActive (true);
			obj.GetComponent<MeshRenderer> ().material = materials [Save.Instance.objectiveTracker[i+1]];
            if(!objectives.contents.Contains(obj))
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
        int colourIndex = objectiveCube.GetComponent<Objective>().colourIndex;

        for (int i = 0; i < sum; i++){
            if (cubeExists) {
                objectiveCube.transform.position = worldPos;
                objectiveCube.transform.SetParent(transform);
                Debug.Log(objectives.contents[Save.Instance.objectiveTracker[0]].ToString());
                yield return StartCoroutine(objectiveCube.GetComponent<ClickableObject>().SmoothMovement(objectives.contents[Save.Instance.objectiveTracker[0]].transform.position, objectives.moveTime, new Vector3 (objectives.objScale, objectives.objScale, objectives.objScale)));
                SendToPool(objectiveCube);
                cubeExists = false;
            }
			objectives.contents [Save.Instance.objectiveTracker[0]].GetComponent<MeshRenderer> ().sharedMaterial = materials[colourIndex];
            Save.Instance.objectiveTracker[0]++;
            Save.Instance.objectiveTracker[Save.Instance.objectiveTracker[0]] = colourIndex;
            if (Save.Instance.objectiveTracker[0] + 1 == Save.Instance.objectiveTracker.Count) {
                yield return StartCoroutine(ObjectiveReward(Save.Instance.objectiveTracker[0]));
			}
		}
	}

    private IEnumerator ObjectiveReward(int total)
    {
        //Make a new list of empty objective slots with one more than the last
        Save.Instance.objectiveTracker.Clear();
        Save.Instance.objectiveTracker.Add(total);
        for (int i = 0; i < total + 1; i++)
            Save.Instance.objectiveTracker.Add(0);

        // Increase max hand size
        yield return StartCoroutine(hand.IncreaseLimit(1));
        yield return NewTracker();
    }

    public GameObject boardObjective(int colourIndex) {
        GameObject obj = GetObject();
        obj.GetComponent<Objective>().colourIndex = colourIndex;
        obj.GetComponent<MeshRenderer>().sharedMaterial = materials[colourIndex];

        Objective o = obj.GetComponent<Objective>();

        switch (colourIndex)
        {
            case 1:
                o.cost = new int[3] { 1, 0, 0 };
                break;
            case 2:
                o.cost = new int[3] { 0, 1, 0 };
                break;
            case 3:
                o.cost = new int[3] { 0, 0, 1 };
                break;
            case 4:
                o.cost = new int[3] { 1, 1, 0 };
                break;
            case 5:
                o.cost = new int[3] { 1, 0, 1 };
                break;
            case 6:
                o.cost = new int[3] { 0, 1, 1 };
                break;
        }

        obj.SetActive(true);

        return obj;
    }

}
