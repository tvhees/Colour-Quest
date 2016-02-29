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

	public static int[] Zip(this int[] a, int[] b, int alpha = 1, int beta = 1){
		int[] c = new int[a.Length];
		for (int i = 0; i < a.Length; i++) {
			c[i] = alpha * a[i] + beta * b[i];	
		}
		return c;
	}

    public static int Sum(this int[] input) {
        int total = 0;
        for (int i = 0; i < input.Length; i++) {
            total += input[i];
        }

        return total;
    }
}
