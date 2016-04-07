using UnityEngine;
using System.Collections;

public abstract class ClickableObject : MonoBehaviour {

    public bool moving = false;

#if UNITY_STANDALONE || UNITY_EDITOR
    public void OnMouseDown() {
        ClickAction();
    }
#endif

	public IEnumerator SmoothMovement(Vector3 target, float moveTime, Vector3 s1){
		float sqrDistance = (transform.position - target).sqrMagnitude;

        float d0 = sqrDistance;
        Vector3 s0 = transform.localScale;

		while (sqrDistance > Mathf.Epsilon) {

            moving = true;

			while (Master.Instance.state != Master.State.GAME)
				yield return new WaitForSeconds(0.1f);

			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

            transform.localScale = Vector3.Lerp(s0, s1, (d0 - sqrDistance) / d0);

			yield return null;
		}

        // Make sure the local scale is correct at the end point in case movement happens too fast for the system.
        transform.localScale = s1;

        moving = false;
	}

    public abstract void ClickAction();

    public virtual void KillTile(bool dead) {

    }
}
