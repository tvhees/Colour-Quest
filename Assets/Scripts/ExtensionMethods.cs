using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

	public static void Randomise<T>(this T[] param){
		for (int i = 0; i < param.Length; i++) {
			var temp = param [i];
			var randomIndex = Random.Range (i, param.Length);
			param [i] = param [randomIndex];
			param [randomIndex] = temp;
		}
	}

	public static void InstantiateChild(this Transform parent, GameObject obj, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)){
		GameObject instance = (GameObject)Object.Instantiate (obj, position, rotation);
		instance.transform.SetParent (parent);
	}
}
