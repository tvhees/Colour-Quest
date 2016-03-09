using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    public Vector3 rotation;
    public float angularSpeed;

	void Update () {
        transform.Rotate(rotation, angularSpeed*Time.deltaTime );	
	}
}
