using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ObjectPool : MonoBehaviour {

    public int poolSize;
	public Vector3 homePosition;

	protected List<GameObject> pool;

	protected void CreatePool(int size, GameObject prefab){
		pool = new List<GameObject>();

		for(int i = 0; i < size; i++){
			GameObject obj = (GameObject)Instantiate(prefab);
			obj.transform.position = homePosition;
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
        obj.transform.position = homePosition;
        obj.transform.localScale = Vector3.one;
        obj.SetActive(false);
	}

	public int GetValueIndex(int[] value){
		int index = 0;
		int incrementer = 1;
		for (int i = 0; i < value.Length; i++) {
			if (value [i] > 0) {
				index += i + incrementer;
				incrementer++;
			}
		}
		return index;
	}

	public abstract void SendToPool(GameObject obj);
}
