using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float period, height, lowPoint;
	public Vector3 offset;
	// Update is called once per frame
	void Update () {
		transform.localPosition = GetComponent<RectTransform>().localScale.x * (offset + new Vector3 (0f, lowPoint + height * (1 - Mathf.Sin (Time.time / period)), 0f));
	}
}
