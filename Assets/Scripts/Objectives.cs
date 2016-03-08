using UnityEngine;
using System.Collections;

public class Objectives : Collection<Objectives> {

	public ObjectivePool pool;

	public void Clear(){
		while (contents.Count > 0) {
			pool.ReturnObject (contents [0]);
			RemoveMana (contents [0]);
		}
	}

}
