using UnityEngine;
using System.Collections;

public class Objectives : Collection<Objectives> {

	public override void Reset ()
	{
		Clear ();

		centered = false;

		translate = new Vector3 (0.5f * objScale * gapScale, 0f, 0f);
	}

	public void Clear(){
		while (contents.Count > 0) {
			pool.SendToPool (contents [0]);
		}
	}
}
