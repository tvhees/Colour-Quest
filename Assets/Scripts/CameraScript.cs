using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Camera thisCamera;
	public Vector3 cameraOffset;
	public float moveTime, zoomSpeed, targetSize;

	public IEnumerator FocusCamera(Transform parentTransform){

		transform.SetParent (parentTransform);

		Vector3 target = parentTransform.position + cameraOffset;

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

    public void Reset(Transform player) {
        transform.SetParent(player);
        thisCamera.orthographicSize = 10f;
        targetSize = 5f;
    }

    void Update() {
        if (thisCamera.orthographicSize > targetSize) {
            thisCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(thisCamera.orthographicSize, targetSize - 1f, zoomSpeed * Time.deltaTime), targetSize, Mathf.Infinity);
        }
    }
}
