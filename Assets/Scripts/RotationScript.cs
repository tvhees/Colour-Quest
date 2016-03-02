using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    private Vector3 rotation;

    void Start() {
        rotation = new Vector3(-1f, 0f, -1f);
    }

	void Update () {
        transform.Rotate(rotation, 80*Time.deltaTime );	
	}
}
