using UnityEngine;
using System.Collections;

public class BoardScript : MonoBehaviour {

	public GameObject[] baseHexes;

	private float zF = 1 / Mathf.Sqrt (3f);

	void Awake(){
		baseHexes.Randomise ();

		transform.InstantiateChild (baseHexes [0], new Vector3 (2f, 0f, 4f * zF), Quaternion.identity);
		transform.InstantiateChild (baseHexes [1], new Vector3 (3f, 0f, -1f * zF), Quaternion.identity);
		transform.InstantiateChild (baseHexes [2], new Vector3 (5f, 0f, 3f * zF), Quaternion.identity);
	}
}