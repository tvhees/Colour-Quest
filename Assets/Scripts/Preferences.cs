using UnityEngine;
using System.Collections;

public class Preferences : Singleton<Preferences> {

	public float cameraSpeed;
	public bool tutorial, watchGoal;

	// Use this for initialization
	public void Load(){
		cameraSpeed = PlayerPrefs.GetFloat ("cameraSpeed", 5.0f);
		tutorial = ExtensionMethods.GetBool ("tutorial", true);
		watchGoal = ExtensionMethods.GetBool ("watchGoal", true);
	}

	public void Save(){
		PlayerPrefs.SetFloat ("cameraSpeed", cameraSpeed);
		ExtensionMethods.SetBool ("tutorial", tutorial);
		ExtensionMethods.SetBool ("watchGoal", watchGoal);
		PlayerPrefs.Save ();
	}

	void OnDisable(){
		Save ();
	}
}
