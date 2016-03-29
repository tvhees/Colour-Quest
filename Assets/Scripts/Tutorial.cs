﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public float[] guiBox;
	public GameObject player, goal;
	public GameObject[] arrows;
	public GUISkin tutorialSkin;
	public ManaPayment manaPayment;
	public CameraScript mainCameraScript;

	private float screenWidth, screenHeight;
	public List<GameObject> offArrows = new List<GameObject>();
	private string clickTag;
	public int tutStep = 0;
	private bool clickAll = false;

	private string[] tutText = new string[15]{
		/*0*/	"Welcome to Colour Quest! This lovely sphere is you, the player.",
		/*1*/	"Your goal is to reach this black sphere and feed it colours until it is satisfied.",
		/*2*/	"To succeed, you'll need to move across the map and gather objectives. Start by touching an adjacent tile to select it.",
		/*3*/	"Selected tiles are highlighted and can be touched again to deselect.",
		/*4*/	"To move on to a new tile you need to feed it the appropriate colour from your hand. Try touching a colour to select it.",
		/*5*/	"The selected colour needs to match the tile - try deselecting non-matching colours and then selecting matching ones.",
		/*6*/	"If all tile requirements have been satisfied, the player sphere will light up. Touch it to confirm and move on to the tile.",
		/*7*/	"Spent colours are sent here. Touching this container will display a list of the colours within, right above the player sphere.",
		/*8*/	"Most tiles are hidden to start with, and will flip as you approach them. You can move and zoom the camera around at any time by dragging or pinching.",
		/*9*/	"Touching here will end your turn and send all unused colours to the spent container. Your turn will also end automatically any time your hand is empty.",
		/*10*/	"At the end of each turn the goal sphere will move one tile, adding that tile's colour requirements to its own.",
		/*11*/	"The goal will always indicate the next tile it will move to",
		/*12*/	"At the start of a new turn your hand will be refilled with colours from the fresh colour stock. Your next hand is always displayed here.",
		/*13*/	"Touching the colour stock will display the colours remaining in there, above your player sphere.",
		/*14*/	"If the stock becomes empty it is refilled with spent colours, which can be used again when they make it in to your hand!"
	};

	public void Reset(){
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		SetArrows (null);

		tutStep = 0;
	}


	private void SetArrows(int[] index){
		offArrows.Clear();
		offArrows.AddRange (arrows);
		if (index != null) {
			for (int i = 0; i < index.Length; i++) {
				arrows [index [i]].SetActive (true);
				offArrows.Remove (arrows [index [i]]);
			}
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
				switch(tutStep){
				case 0:	// Introducing the game, player sphere and goal sphere
					clickTag = null;
					SetArrows (new int[1]{ 0 });
					break;
				case 1:
					StartCoroutine(mainCameraScript.FocusCamera (goal.transform));
					SetArrows (new int[1]{ 1 });
					break;
				case 2:		// Ask the player to select an adjacent tile
					StartCoroutine(mainCameraScript.FocusCamera (player.transform));
					clickTag = "Tile";
					break;
				default:	// If we're not at an appropriate stage of the tutorial we allow full functionality
					clickTag = "None";
					clickAll = true;
					SetArrows(null);
					break;
				}
				break;
			case Game.State.PAYING: // This is for when players have selected a tile but not confirmed movement
				switch (tutStep) {
				case 3:		// Explain tiles are highlighted and can be deselected
					clickTag = null;
					break;
				case 4:		// Ask the player to select an appropriate colour of mana
					SetArrows(new int[1]{ 2 });
					clickTag = "Mana";
					break;
				case 5:		// Warn player if wrong colour combination has been selected, otherwise immediately progress to next step
					if (manaPayment.payed)
						tutStep++;
					break;
				case 6:		// Ask the player to confirm payment by pressing the player sphere
					if (manaPayment.payed) {
						SetArrows (new int[1]{ 0 });
						clickTag = "Player";
					}
					else
						tutStep--;
					break;
				default:
					clickTag = "None";
					clickAll = true;
					SetArrows(null);
					break;
				}
				break;
			case Game.State.GOAL:
				switch(tutStep){
				default:
					clickTag = "None";
					clickAll = true;
					SetArrows(null);
					break;
				}
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
		// Only increment tutorials on click, otherwise release counts double immediately
		if (message == "ClickAction") {
			// Automatically turn off tutorial mode if we've gone all the way through.
			if (tutStep > 10) {
				Preferences.Instance.tutorial = false;
			}

			// This is for messages that don't request specific targets from players
			if (clickTag == null) {
				tutStep++;
				return;
			}

			// This is for messages that ask for specific objects to be clicked
			if (hit.transform.tag == clickTag) {
				tutStep++;
			} else if(!clickAll)
				return;
		}


		hit.SendMessage (message);
	}
}
