using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Goal : MovingObject<Goal> {

	public int leftMax, rightMax;
	public GameObject goalMarker, player, gameCamera;
    public GoalObject goalObject;
    public Vector3 goalTarget;
    public TextMesh[] goalValue;
    public bool pause;

	private GameObject goalTile = null;
    private bool canMove, gameStarted;
	private Vector3 leftVector = new Vector3(1f, 0f, -1f/Mathf.Sqrt(3f)), rightVector = new Vector3(1f, 0f, 1f/Mathf.Sqrt(3f)), raycastOffset = new Vector3(0f, 1f, 0f);
	private List<bool> directionList = new List<bool>();

    public override void Reset()
    {
        transform.position = startLocation;
        goalMarker.SetActive(true);
        canMove = true;

        gameStarted = false;
        goalObject.goalCost = new int[3] { 0, 0, 0 };

        int[] startCost = new int[3] { 0, 0, 0 };

        // Increase goalCost based on difficulty
        for (int i = 0; i < Preferences.Instance.difficulty; i++) {
            int j = (int)Mathf.Repeat(i, 3);
            startCost[j]++;
        }

        UpdateValue(startCost);

        gameStarted = true;

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

		if(Preferences.Instance.watchGoal) // Set in player preferences
			yield return StartCoroutine(gameCamera.GetComponent<CameraScript> ().FocusCamera (transform));

        while (pause)
            yield return new WaitForSeconds(0.1f);

        // If there's still tiles to move to, continue. If not, end the game as a loss
        if (canMove)
        {
            // Grab value of the next tile to be consumed and add it to the goal value
            UpdateValue(goalTile.GetComponent<TileScript>().tileCost);

            // Move on to the tile and destroy it
            if (Preferences.Instance.watchGoal)
                yield return StartCoroutine(SmoothMovement(goalTarget, goalTile.transform.parent.gameObject));
            else
                InstantMovement(goalTarget, goalTile.transform.parent.gameObject);

            // If we've moved on top of the player, end the game as a loss
            if ((transform.position - player.transform.position).sqrMagnitude < 0.1f)
            {
                Game.Instance.state = Game.State.LOST;
                Preferences.Instance.UpdateDifficulty(-1);
                player.GetComponent<Player>().childRenderer.enabled = false;
            }
            else
            {
                canMove = NextTile();

                if (Preferences.Instance.watchGoal)
                { // Set in player preferences
                    yield return new WaitForSeconds(1.0f);

                    yield return StartCoroutine(gameCamera.GetComponent<CameraScript>().FocusCamera(player.transform));
                }

                Game.Instance.state = Game.State.IDLE;
            }
        }
        else {
            Game.Instance.state = Game.State.LOST;
            Preferences.Instance.UpdateDifficulty(-1);
        }

		yield return null;
	}

	private bool NextTile(){
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
				return true;
			}
		}

        goalMarker.SetActive(false);

        return false;
	}

    public void UpdateValue(int[] value, int alpha = 1, int beta = 1) {
        goalObject.goalCost = goalObject.goalCost.Zip(value, alpha, beta);

        bool defeated = true;

        for (int i = 0; i < 3; i++) {
            if (goalObject.goalCost[i] > 0)
                defeated = false;
            else if (goalObject.goalCost[i] < 0)
                goalObject.goalCost[i] = 0;

            goalValue[i].text = goalObject.goalCost[i].ToString();
        }

        if (defeated && gameStarted)
        {
            Game.Instance.state = Game.State.WON;
            Preferences.Instance.UpdateDifficulty(2);
        }
    }
}
