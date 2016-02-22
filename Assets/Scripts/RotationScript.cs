using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    private Vector3 rotation;

    void Start() {
        rotation = Random.onUnitSphere;
    }

	void Update () {
        transform.Rotate(rotation, 80*Time.deltaTime );	
	}
}
