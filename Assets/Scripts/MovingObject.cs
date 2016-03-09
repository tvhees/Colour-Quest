using UnityEngine;
using System.Collections;

public abstract class MovingObject<T>: Singleton<T> where T : MonoBehaviour {

    public bool killsTiles;
	public float moveTime;
    public Vector3 startLocation;

    public abstract void Reset();

	public IEnumerator SmoothMovement(Vector3 target, GameObject newTile){
		float sqrDistance = (transform.position - target).sqrMagnitude;

		while (sqrDistance > Mathf.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}
        newTile.GetComponentInChildren<TileScript>().KillTile(killsTiles);
	}
}
