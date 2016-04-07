using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

    public Game game;
    public bool killsTiles;
	public float moveTime;
    public Vector3 startLocation;

    public abstract void Reset();

	public IEnumerator SmoothMovement(Vector3 target, GameObject newTile){
		float sqrDistance = (transform.position - target).sqrMagnitude;

		while (sqrDistance > Mathf.Epsilon) {

            while (Master.Instance.state == Master.State.MENU)
                yield return new WaitForSeconds(0.1f);

			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}
        newTile.GetComponentInChildren<ClickableObject>().KillTile(killsTiles);
	}

	public void InstantMovement(Vector3 target, GameObject newTile){
		transform.position = target;

		newTile.GetComponentInChildren<ClickableObject>().KillTile(killsTiles);
	}
}
