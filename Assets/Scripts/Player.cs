using UnityEngine;
using System.Collections;

public class Player: Singleton<Player> {

	public float moveTime, moveDistance;

	public IEnumerator SmoothMovement(Vector3 target, GameObject newTile){
		float sqrDistance = (transform.position - target).sqrMagnitude;

		if (sqrDistance < moveDistance) {
			while (sqrDistance > Mathf.Epsilon) {
				Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

				transform.position = newPosition;

				sqrDistance = (transform.position - target).sqrMagnitude;

				yield return null;
			}

			Destroy (newTile);
		}
	}
}
