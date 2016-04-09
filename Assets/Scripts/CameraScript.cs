﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public Game game;
    public Camera thisCamera;
    public Camera uiCamera;
	public Vector3 cameraOffset, lastPosition;
	public float zoomRatio, panRatio, lastTouch, returnDelay, targetSize;
	public RectTransform touchZone;
	public TextMesh touchPosition, isInTouchZone;
	public Game.State savedState;

#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
    private bool inTouchZone;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
    private bool firstPan;
#endif

    public IEnumerator FocusCamera(Transform parentTransform){

		transform.SetParent (parentTransform);

		Vector3 target = parentTransform.position + cameraOffset;

		float sqrDistance = (transform.position - target).sqrMagnitude;

		savedState = game.state;

		game.state = Game.State.CAMERA;

		while (sqrDistance > Mathf.Epsilon) {

            while (Master.Instance.state != Master.State.GAME)
                yield return new WaitForSeconds(0.1f);

			Vector3 newPosition = Vector3.MoveTowards (transform.position, target, 2 * Time.deltaTime * Preferences.Instance.cameraSpeed);

			transform.position = newPosition;

			sqrDistance = (transform.position - target).sqrMagnitude;

			yield return null;
		}

		game.state = savedState;
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

		if (game.state == Game.State.IDLE || game.state == Game.State.PAYING) {
			// Camera pan controlling code
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && CheckTouchZone(Input.GetTouch(0).position)){
	            inTouchZone = true;
			}

	        if (Input.touchCount == 0) {
	            inTouchZone = false;
	        }

			isInTouchZone.text = inTouchZone.ToString ();
			if(Input.touchCount > 0)
				touchPosition.text = Input.GetTouch(0).position.ToString();
			else
				touchPosition.text = "No touch recorded";

	        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
	        {
	            if (inTouchZone)
	            {
	                lastTouch = Time.time;
	                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.Translate(-touchDeltaPosition.x * panRatio * Preferences.Instance.cameraSpeed, -touchDeltaPosition.y * panRatio * Preferences.Instance.cameraSpeed, 0);
	            }
	        }          

	        // Camera zoom controlling code
	        // If there are two touches on the device...
	        if (Input.touchCount == 2)
	        {
	            if (inTouchZone)
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
			thisCamera.orthographicSize += zoomRatio * deltaMagnitudeDiff * Preferences.Instance.cameraSpeed;

	                // Make sure the orthographic size never drops below zero.
	                thisCamera.orthographicSize = Mathf.Max(thisCamera.orthographicSize, targetSize - 1);
	            }
	        }
#endif

#if UNITY_STANDALONE || UNITY_EDITOR
			if (Input.GetMouseButton (1)) {
				if (firstPan) {
					lastPosition = Input.mousePosition;
					firstPan = false;
				}
				lastTouch = Time.time;

				Vector2 touchDeltaPosition = Input.mousePosition - lastPosition;
				transform.Translate (-touchDeltaPosition.x * panRatio * Preferences.Instance.cameraSpeed, -touchDeltaPosition.y * panRatio * Preferences.Instance.cameraSpeed, 0);

				lastPosition = Input.mousePosition;
			} else
				firstPan = true;

			float deltaMagnitudeDiff = Input.mouseScrollDelta.y;

			// if the scroll wheel has been moved
			if (deltaMagnitudeDiff != 0) {
				// Store the last time zoom was requested
				lastTouch = Time.time;

				// change the orthographic size based on the change scroll wheel position.
				thisCamera.orthographicSize += -deltaMagnitudeDiff * Preferences.Instance.cameraSpeed;

				// Make sure the orthographic size never drops below zero.
				thisCamera.orthographicSize = Mathf.Max (thisCamera.orthographicSize, targetSize - 1);

			}
#endif

			if (Time.time - lastTouch > returnDelay) {
				transform.localPosition = Vector3.MoveTowards (transform.localPosition, cameraOffset, Preferences.Instance.cameraSpeed * Time.deltaTime);
				thisCamera.orthographicSize = Mathf.MoveTowards (thisCamera.orthographicSize, targetSize, Preferences.Instance.cameraSpeed * Time.deltaTime);
			}
		}
    }

    private bool CheckTouchZone(Vector2 touchPoint) {
        return RectTransformUtility.RectangleContainsScreenPoint(touchZone, touchPoint, uiCamera);
    }
}
