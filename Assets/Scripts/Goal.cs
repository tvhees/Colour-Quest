using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Goal : MovingObject<Goal> {

    public TileScript tileScript;
	public int leftMax, rightMax;
	public GameObject goalMarker, player, mainCamera;
	public Vector3 goalTarget;
    public TextMesh[] goalValue;

	public GameObject goalTile = null;
	private Vector3 leftVector = new Vector3(1f, 0f, -1f/Mathf.Sqrt(3f)), rightVector = new Vector3(1f, 0f, 1f/Mathf.Sqrt(3f)), raycastOffset = new Vector3(0f, 1f, 0f);
	private List<bool> directionList = new List<bool>();

    public override void Reset()
    {
        transform.position = startLocation;

        tileScript.tileCost = new int[3] { 0, 0, 0 };
        UpdateValue(tileScript.tileCost);

        bool[] temp = new bool[leftMax + rightMax];

        for (int i = 0; i < leftMax; i++)
            temp[i] = true;

        for (int i = leftMax; i < temp.Length; i++)
            temp[i] = false;

        temp.Randomise();
        directionList.AddRange(temp);

        NextTile();
    }

    public IEnumerator MoveGoal(){
		Game.Instance.state = Game.State.GOAL;

		yield return StartCoroutine(mainCamera.GetComponent<CameraScript> ().FocusCamera (transform));

        tileScript.tileCost = tileScript.tileCost.Zip(goalTile.GetComponent<TileScript>().tileCost);

        UpdateValue(tileScript.tileCost);

		yield return StartCoroutine(SmoothMovement(goalTarget, goalTile.transform.parent.gameObject));

		NextTile ();

		yield return new WaitForSeconds (1.0f);

		yield return StartCoroutine(mainCamera.GetComponent<CameraScript> ().FocusCamera (player.transform));

		Game.Instance.state = Game.State.IDLE;

		yield return null;
	}

	private void NextTile(){
		RaycastHit hit;
		goalTarget = Vector3.zero;

		for (int i = 0; i < directionList.Count; i++) {
			if (directionList [i])
				goalTarget = transform.position - leftVector - raycastOffset;
			else
				goalTarget = transform.position - rightVector - raycastOffset;

			Physics.Raycast (goalTarget, Vector3.up, out hit);

			if(hit.collider != null){
				goalTile = hit.collider.gameObject;
                goalTarget = goalTarget + raycastOffset;
				goalMarker.transform.position = goalTarget;
				directionList.RemoveAt (i);
				break;
			}
		}
	}

    private void UpdateValue(int[] value) {
        for (int i = 0; i < 3; i++) {
            goalValue[i].text = value[i].ToString();
        }
    }

}
