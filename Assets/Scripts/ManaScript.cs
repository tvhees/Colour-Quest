using UnityEngine;
using System.Collections;

public class ManaScript : MonoBehaviour {

	public Material[] materials;

	void Awake(){
		materials.Randomise ();

		GetComponent<MeshRenderer> ().material = materials [0];
	}
}
