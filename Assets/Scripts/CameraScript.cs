using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Camera thisCamera;
	public Vector3 cameraOffset;
	public float moveTime, zoomSpeed, zoomTime, zoomDelay, targetSize;

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
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store the last time zoom was requested
            zoomTime = Time.time;

            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // change the orthographic size based on the change in distance between the touches.
            thisCamera.orthographicSize += deltaMagnitudeDiff * zoomSpeed;

            // Make sure the orthographic size never drops below zero.
            thisCamera.orthographicSize = Mathf.Max(thisCamera.orthographicSize, targetSize);

        }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        float deltaMagnitudeDiff = Input.mouseScrollDelta.y;

        // if the scroll wheel has been moved
        if (deltaMagnitudeDiff != 0)
        {
            // Store the last time zoom was requested
            zoomTime = Time.time;

            // change the orthographic size based on the change scroll wheel position.
            thisCamera.orthographicSize += -deltaMagnitudeDiff * zoomSpeed;

            // Make sure the orthographic size never drops below zero.
            thisCamera.orthographicSize = Mathf.Max(thisCamera.orthographicSize, targetSize);

        }
#endif
        else if (thisCamera.orthographicSize > targetSize)
        {
            if(Time.time - zoomTime > zoomDelay)
                thisCamera.orthographicSize = Mathf.Clamp(Mathf.Lerp(thisCamera.orthographicSize, targetSize - 1f, zoomSpeed * Time.deltaTime), targetSize, Mathf.Infinity);
        }
    }
}
