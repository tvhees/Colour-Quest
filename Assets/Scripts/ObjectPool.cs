using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectPool : MonoBehaviour {

	protected List<GameObject> pool;

	protected void CreatePool(int size, GameObject prefab){
		pool = new List<GameObject>();

		for(int i = 0; i < size; i++){
			GameObject obj = (GameObject)Instantiate(prefab);
			obj.SetActive (false);
			pool.Add(obj);
		}
	}

	protected GameObject GetObject(){
		if(pool.Count > 0){
			GameObject obj = pool [0];
			pool.RemoveAt(0);

			return obj;
		}

		return null;
	}

	protected void SendToPool(GameObject obj){
		pool.Add(obj);
		obj.SetActive(false);
	}

}
