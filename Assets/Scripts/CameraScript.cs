using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public Vector3 cameraOffset;
	public float moveTime;

	public IEnumerator FocusCamera(Transform parentTransform){

		transform.SetParent (parentTransform);

		Vector3 target = parentTransform.position + cameraOffset;

		float sqrDistance = (transform.position - target).sqrMagnitude;

		while (sqrDistance > Mathf.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime / moveTime);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}
	}
}
