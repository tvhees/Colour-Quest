using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        /// <summary>
        /// Add two int arrays together index by index.
        /// alpha is the multiplyer for the calling array, beta the multiplyer for the sent array.
        /// </summary>
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

	// Adding bool functions to PlayerPrefs
	public static void SetBool(string name, bool booleanValue) 
	{
		PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
	}

	public static bool GetBool(string name)  
	{
		return PlayerPrefs.GetInt(name) == 1 ? true : false;
	}

	public static bool GetBool(string name, bool defaultValue)
	{
		if(PlayerPrefs.HasKey(name)) 
		{
			return GetBool(name);
		}

		return defaultValue;
	}

    // Make text with a different colour background
    public static void BackgroundText(Rect rect, string textString, GUISkin textSkin, bool background = false) {
        float tO = 1f;
        if(background)
            GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), "", textSkin.customStyles[1]);
        GUI.Label(new Rect(rect.x + tO, rect.y + tO, rect.width, rect.height), textString);
        GUI.Label(new Rect(rect.x + tO, rect.y - tO, rect.width, rect.height), textString);
        GUI.Label(new Rect(rect.x - tO, rect.y + tO, rect.width, rect.height), textString);
        GUI.Label(new Rect(rect.x - tO, rect.y - tO, rect.width, rect.height), textString);
        GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), textString, textSkin.customStyles[0]);
    }

    public static bool BGButton(Rect rect, string textString, GUISkin textSkin) {
        bool value = GUI.Button(rect, "");
        BackgroundText(rect, textString, textSkin);
        return value;
    }
}
