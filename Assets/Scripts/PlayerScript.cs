using UnityEngine;
using System.Collections;

public class PlayerScript: MonoBehaviour {

	public float moveTime;
	public float moveDistance;
	public GameObject currentTile;

	public void ChangeTile(GameObject newTile){
		Destroy (currentTile);
		currentTile = newTile;
	}

	public IEnumerator SmoothMovement(Vector3 target, GameObject newTile){
		float sqrDistance = (transform.position - target).sqrMagnitude;

		if (sqrDistance < moveDistance) {
			if (newTile != currentTile) {
				while (sqrDistance > Mathf.Epsilon) {
					Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

					transform.position = newPosition;

					sqrDistance = (transform.position - target).sqrMagnitude;

					yield return null;
				}

				ChangeTile (newTile);
			}
		}
	}
}
