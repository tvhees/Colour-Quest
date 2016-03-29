using UnityEngine;
using System.Collections;

public abstract class ClickableObject : MonoBehaviour {

	public float moveTime;

#if UNITY_STANDALONE || UNITY_EDITOR
    public void OnMouseDown() {
		// If the tutorial is running we want extra control over what happens
		if(Preferences.Instance.tutorial){
			if(Game.Instance.state == Game.State.IDLE || Game.Instance.state == Game.State.PAYING || Game.Instance.state == Game.State.GOAL){
				Game.Instance.tutorial.ClickAction(transform, "ClickAction");
			}
		}
		else 
	        ClickAction();
    }
#endif

	public IEnumerator SmoothMovement(Vector3 target){
		float sqrDistance = (transform.position - target).sqrMagnitude;

		while (sqrDistance > Mathf.Epsilon) {

			while (Game.Instance.state == Game.State.MENU)
				yield return new WaitForSeconds(0.1f);

			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}
	}

    public abstract void ClickAction();

    public virtual void KillTile(bool dead) {

    }
}
