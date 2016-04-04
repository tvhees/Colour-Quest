using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

	public float cameraSpeed;
    public int difficulty;
	public bool tutorial, watchGoal;
    public Slider cameraSlider;
    public Toggle tutorialMode, watchMode;
    public TextMesh difficultyMesh;

	// Use this for initialization
	public void Load(){
		cameraSpeed = PlayerPrefs.GetFloat ("cameraSpeed", 5.0f);
		tutorial = ExtensionMethods.GetBool ("tutorial", true);
		watchGoal = ExtensionMethods.GetBool ("watchGoal", true);
        difficulty = PlayerPrefs.GetInt("difficulty", 4);

        UpdateDifficulty(0);

        cameraSlider.value = cameraSpeed;
        tutorialMode.isOn = tutorial;
        watchMode.isOn = watchGoal;
	}

    public void Reset() {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public void UpdateCamera() {
        cameraSpeed = cameraSlider.value;
    }

    public void UpdateTutorialMode() {
        tutorial = tutorialMode.isOn;
    }

    public void UpdateDifficulty(int adjustment) {
        difficulty = Mathf.Max(difficulty + adjustment, 0);
        difficultyMesh.text = difficulty.ToString();
        PlayerPrefs.Save();
    }

    public void UpdateWatchMode()
    {
        watchGoal = watchMode.isOn;
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
