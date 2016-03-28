using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public float[] guiBox;
	public string[] tutText;
	public GameObject[] arrows;
	public GUISkin tutorialSkin;

	private float screenWidth, screenHeight;
	private List<GameObject> offArrows = new List<GameObject>();
	private string clickTag;
	private int tutStep = 0;
	private bool clickAll = false;

	void Start(){
		screenWidth = Screen.width;
		screenHeight = Screen.height;
	}

	void Update () {
		// Tutorial arrow management
		if(Preferences.Instance.tutorial){
			switch (Game.Instance.state) {
			case Game.State.IDLE:
				SetArrows (new int[1]{0});
				break;
			case Game.State.PAYING:
				SetArrows (new int[3]{2, 4, 5});
				break;
			case Game.State.GOAL:
				SetArrows (new int[1]{ 1 });
				break;
			}
		}
		else{
			for(int i = 0; i < arrows.Length; i++){
				arrows[i].SetActive(false);
			}
		}
	}

	private void SetArrows(int[] index){
		offArrows.Clear();
		offArrows.AddRange (arrows);
		for (int i = 0; i < index.Length; i++) {
			arrows [index [i]].SetActive (true);
			offArrows.Remove (arrows [index [i]]);
		}

		for (int i = 0; i < offArrows.Count; i++) {
			offArrows [i].SetActive (false);
		}
	}

	void OnGUI(){
		GUI.skin = tutorialSkin;

		if (Preferences.Instance.tutorial) {
			clickAll = false;

			switch (Game.Instance.state) {
			case Game.State.IDLE:
				if (tutStep == 0)
					clickTag = null;
				else if (tutStep == 1)
					clickTag = "Tile";
				else
					clickAll = true;
				break;
			case Game.State.PAYING:
				if (tutStep == 2)
					clickTag = "Mana";
				else if (tutStep == 3)
					clickTag = "Player";
				else
					clickAll = true;
				break;
			case Game.State.GOAL:
				clickAll = true;
				break;
			} 
			if(!clickAll)
				GUI.Box (new Rect (screenWidth * guiBox [0], screenHeight * guiBox [1], screenWidth * guiBox [2], screenHeight * guiBox [3]), tutText[tutStep]);
		}
		else{
			tutStep = 0;
		}
	}

	public void ClickAction(RaycastHit hit, string message){
		if (message == "ClickAction") {
			if (tutStep > 3) {
				Preferences.Instance.tutorial = false;
			}

			tutStep++;

			if (clickTag == null) {
				return;
			}
		}

		if (message != null)
			if (clickAll || hit.transform.tag == clickTag) {
				hit.transform.SendMessage (message);
			}
	}
}
