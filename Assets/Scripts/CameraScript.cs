using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Camera thisCamera;
	public Vector3 cameraOffset, lastPosition;
	public float zoomSpeed, zoomRatio, panSpeed, panRatio, lastTouch, returnDelay, targetSize;

#if UNITY_EDITOR || UNITY_STANDALONE
    private bool firstPan;
#endif

    public IEnumerator FocusCamera(Transform parentTransform){

		transform.SetParent (parentTransform);

		Vector3 target = parentTransform.position + cameraOffset;

		float sqrDistance = (transform.position - target).sqrMagnitude;

		while (sqrDistance > Mathf.Epsilon) {

            while (Game.Instance.state == Game.State.MENU)
                yield return new WaitForSeconds(0.1f);

            Vector3 newPosition = Vector3.MoveTowards (transform.position, target, Time.deltaTime * panSpeed);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}
	}

    public void Reset(Transform player) {
        transform.SetParent(player);
        thisCamera.orthographicSize = 10f;
        targetSize = 5f;
#if UNITY_EDITOR || UNITY_STANDALONE
        firstPan = true;
#endif
    }

    void Update() {
        // Camera pan controlling code
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            lastTouch = Time.time;
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * panRatio * panSpeed, -touchDeltaPosition.y * panRatio * panSpeed, 0);
        }          

        // Camera zoom controlling code
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store the last time zoom was requested
            lastTouch = Time.time;

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
            thisCamera.orthographicSize += zoomRatio * deltaMagnitudeDiff * zoomSpeed;

            // Make sure the orthographic size never drops below zero.
            thisCamera.orthographicSize = Mathf.Max(thisCamera.orthographicSize, targetSize - 1);

        }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(1))
        {
            if (firstPan)
            {
                lastPosition = Input.mousePosition;
                firstPan = false;
            }
            lastTouch = Time.time;

            Vector2 touchDeltaPosition = Input.mousePosition - lastPosition;
            transform.Translate(-touchDeltaPosition.x * panRatio * panSpeed, -touchDeltaPosition.y * panRatio * panSpeed, 0);

            lastPosition = Input.mousePosition;
        }
        else
            firstPan = true;

        float deltaMagnitudeDiff = Input.mouseScrollDelta.y;

        // if the scroll wheel has been moved
        if (deltaMagnitudeDiff != 0)
        {
            // Store the last time zoom was requested
            lastTouch = Time.time;

            // change the orthographic size based on the change scroll wheel position.
            thisCamera.orthographicSize += -deltaMagnitudeDiff * zoomSpeed;

            // Make sure the orthographic size never drops below zero.
            thisCamera.orthographicSize = Mathf.Max(thisCamera.orthographicSize, targetSize - 1);

        }
#endif

        if (Time.time - lastTouch > returnDelay)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, cameraOffset, panSpeed * Time.deltaTime);
            thisCamera.orthographicSize = Mathf.MoveTowards(thisCamera.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
        }
    }
}
