using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public float[] guiBox;
	public GameObject[] arrows;
	public GUISkin tutorialSkin;

	private float screenWidth, screenHeight;
	private List<GameObject> offArrows = new List<GameObject>();
	private string clickTag;
	private int tutStep = 0;
	private bool clickAll = false;

	private string[] tutText = new string[14]{
		/*0*/	"Welcome to Colour Quest! This lovely sphere is you, the player.",
		/*1*/	"Your goal is to reach this black sphere and feed it colours until it is satisfied.",
		/*2*/	"To succeed, you'll need to move across the map and gather objectives. Start by touching an adjacent tile to select it.",
		/*3*/	"Selected tiles are highlighted and can be touched again to deselect.",
		/*4*/	"To move on to a new tile you need to feed it the appropriate colour from your hand. Try touching a colour to select it.",
		/*5*/	"If all tile requirements have been satisfied, the player sphere will light up. Touch it to confirm and move on to the tile.",
		/*6*/	"Spent colours are sent here. Touching this container will display a list of the colours within, right above the player sphere.",
		/*7*/	"Most tiles are hidden to start with, and will flip as you approach them. You can move and zoom the camera around at any time by dragging or pinching.",
		/*8*/	"Touching here will end your turn and send all unused colours to the spent container. Your turn will also end automatically any time your hand is empty.",
		/*9*/	"At the end of each turn the goal sphere will move one tile, adding that tile's colour requirements to its own.",
		/*10*/	"The goal will always indicate the next tile it will move to",
		/*11*/	"At the start of a new turn your hand will be refilled with colours from the fresh colour stock. Your next hand is always displayed here.",
		/*12*/	"Touching the colour stock will display the colours remaining in there, above your player sphere.",
		/*13*/	"If the stock becomes empty it is refilled with spent colours, which can be used again when they make it in to your hand!"
	};

	public void Reset(){
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		tutStep = 0;
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
			case Game.State.IDLE: // The idle state covers situations where no tile has been clicked, player input is expected and so on
				// Some messages are just displaying information - we want touches and clicks to progress the message only
				if (tutStep == 0 || tutStep == 1)
					clickTag = null;
				// This is for messages instructing the player to select an adjacent tile
				else if (tutStep == 2)
					clickTag = "Tile";
				// If we're not at an appropriate stage of the tutorial we allow full functionality
				else
					clickAll = true;
				break;
			case Game.State.PAYING: // This is for when players have selected a tile but not confirmed movement
				if (tutStep == 3)
					clickTag = null;
				// For messages instructing the player to select a mana colour
				else if (tutStep == 4)
					clickTag = "Mana";
				// For messages instructing the player to touch the player object
				else if (tutStep == 5)
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

	public void ClickAction(Transform hit, string message){
		if (message == "ClickAction") {
			if (tutStep > 5) {
				Preferences.Instance.tutorial = false;
			}

			tutStep++;

			if (clickTag == null) {
				return;
			}
		}

		if (message != null)
		if (clickAll || hit.transform.tag == clickTag) {
			hit.SendMessage (message);
		}
	}
}
