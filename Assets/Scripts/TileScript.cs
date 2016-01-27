using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public Material[] materials;

	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		meshRenderer.sharedMaterial = materials [Random.Range (0, materials.Length)];
	}
}
