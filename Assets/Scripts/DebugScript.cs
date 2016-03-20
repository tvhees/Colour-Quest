using UnityEngine;
using System.Collections;

public class DebugScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		GetComponent<TextMesh> ().text = Screen.currentResolution.ToString();
	}
}
