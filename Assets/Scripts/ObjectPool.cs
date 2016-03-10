using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectPool : MonoBehaviour {

    public int poolSize;

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

            poolSize = pool.Count;

            return obj;
		}

		return null;
	}

	public void ReturnObject(GameObject obj){
		pool.Add(obj);
        obj.transform.SetParent(null);
        obj.transform.position = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(false);
	}

}
