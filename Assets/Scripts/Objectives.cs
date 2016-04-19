using UnityEngine;
using System.Collections;

public class Objectives : Collection {

	public override void Reset ()
	{
		Clear ();

		centered = false;

		translate = new Vector3 (0.5f * objScale * gapScale, 0f, 0f);
	}

    protected override void RemoveFromSave(int index)
    {
        Save.Instance.objectives[index] = -1;
    }

    public void Clear(){
		while (contents.Count > 0) {
			pool.SendToPool (contents [0]);
		}
	}
}
