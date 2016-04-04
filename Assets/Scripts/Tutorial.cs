using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public float[] guiBox;
	public GameObject player, tutorialPanel, twoDPanel;
    public Text tutorialText;
    public Goal goal;
	public GameObject[] arrows;
	public GUISkin tutorialSkin;
	public ManaPayment manaPayment;
	public CameraScript mainCameraScript;
    public Button scrubButton, tutNextButton;

	public List<GameObject> offArrows = new List<GameObject>();
	private string clickTag;
	public int tutStep = 0;
	private bool clickAll = false, release = false, scrubIncrement = false;

    private string[] tutText = new string[26]{
		/*0*/	"Welcome to Colour Quest! This sphere is you, the PLAYER.",
		/*1*/	"Your must reach this GOAL sphere and FEED it colours until it is satisfied.",
		/*2*/	"To succeed, you'll need to move across the map. Start by touching an adjacent TILE to select it.",
		/*3*/	"Selected tiles are highlighted and can be touched again to deselect.",
		/*4*/	"To move on to a new tile you need to feed it the appropriate COLOUR from your hand. Try touching a colour to select it.",
		/*5*/	"The selected colour needs to match the tile - deselect non-matching colours by touching them and try again.",
		/*6*/	"If all tile requirements have been satisfied, the player sphere will light up. Touch it to confirm and move on to the tile.",
		/*7*/	"SPENT colours are sent here. Long-pressing this container will display a list of the colours within, right above the player sphere. Try it now.",
		/*8*/	"Most tiles are hidden to start with, and will flip as you approach them. You can move and zoom the CAMERA around at any time by dragging or pinching.",
		/*9*/	"Touching here will END your turn and send all unused colours to the spent container. Your turn will also end automatically any time your hand is empty.",
		/*10*/	"At the end of each turn the goal sphere will move one tile, adding that tile's colour requirements to its own.",
		/*11*/	"The goal always shows which tile it will move to next with a black smoke marker.",
		/*12*/	"At the start of a new turn your hand will be refilled with colours from the fresh colour STOCK. Your next hand is always displayed next to it.",
		/*13*/	"Long-pressing the colour stock will display any other colours remaining in there. Try it now.",
		/*14*/	"If the stock becomes empty it is refilled with spent colours, which can be used again when they make it in to your hand!",
        /*15*/  "Some tiles have square OBJECTIVE markers above them. To move on to these tiles you must feed them the objective colour as well as the tile colour.",
        /*16*/  "Collected objectives are sent to the objective TRACKER. Filling this up will increase your maximum hand size by one.",
        /*17*/  "RED objectives will add two to your tracker, while YELLOW objectives will put a new red colour in to your hand immediately.",
        /*18*/  "Sometimes you won't have the right colours in your hand. You can change a colour to any other by long-pressing it and selecting the new colour. Try this now.",
        /*19*/  "Beware - this will add one or more BLACK colours to your hand. Black colours cannot be used to move and won't leave your hand unless you skip an entire turn.",
        /*20*/  "Eventually you will encounter PURPLE, GREEN and ORANGE tiles and objectives. These count as the two colours you would mix to form them.",
        /*21*/  "You can also acquire these colours to your hand. They can be used to satisfy either or both of the colours they count as - so a PURPLE can be used to feed a red, blue, or purple requirement!",
        /*22*/  "Winning involves feeding the goal the colours it wants, which are displayed above it at all times. The goal acts like a tile, so you'll need to move next to it and then select it first.",
        /*23*/  "Unlike a tile, you don't have to satisfy ALL of the goal's colour requirements at once - pay any amount and the goal's requirements will decrease accordingly.",
        /*24*/  "Beware, though: For every colour you DON'T feed the goal, it will add one BLACK colour to your hand. If the number of black colours in your hand exceeds your maximum hand size at the end of any turn, you will lose!",
        /*25*/  "That's it for the tutorial - you can turn it on again in the preferences menu if you need a refresher. Have fun!"
	};

	public void Reset(){
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

	void Update(){
		if (Preferences.Instance.tutorial && Game.Instance.state != Game.State.SPLASH) {
			clickAll = false;
            release = false;
            scrubButton.interactable = false;
            scrubIncrement = false;
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
                        case 8:     // Explain Tile Flipping and Camera Movement
                            clickTag = null;
                            break;
                        case 9:     // Explain ending turn and discarding hand
                            clickTag = "EndTurn";
                            SetArrows(new int[1] { 7 });
                            scrubButton.interactable = true;
                            scrubIncrement = true;
                            goal.pause = true;
                            break;
                        case 12:    // Explain hand from stock
                            clickTag = null;
                            SetArrows(new int[1] { 3 });
                            break;
                        case 13:    // Explain stock and display
                            clickTag = "Deck";
                            release = true;
                            SetArrows(new int[1] { 4 });
                            break;
                        case 14:    // Explain refilling stock from discard
                            clickTag = null;
                            SetArrows(null);
                            break;
                        case 15:    // Explaining objectives
                            clickTag = null;
                            break;
                        case 16:    // Objective tracker
                            SetArrows(new int[1] { 6 });
                            clickTag = null;
                            break;
                        case 17:    // Objective bonuses
                            SetArrows(null);
                            clickTag = null;
                            break;
                        case 20:    // Double colour tiles and objectives
                            clickTag = null;
                            break;
                        case 21:    // Double colour mana
                            goal.pause = true;
                            clickTag = null;
                            break;
                        default:	// If we're not at an appropriate stage of the tutorial we allow full functionality
					        clickTag = "None";
					        clickAll = true;
                            tutorialPanel.SetActive(false);
                            twoDPanel.SetActive(false);
                            SetArrows(null);
                            scrubButton.interactable = true;
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
                                manaPayment.pause = true;
                            }
					        else
						        tutStep--;
					        break;
                        case 7:     // Show the player where spent mana goes
                            SetArrows(new int[1] { 5 });
                            clickTag = "Discard";
                            release = true;
                            break;
                        case 18:    // Explain long-pressing to change colours
                            clickTag = "Mana";
                            release = true;
                            break;
                        case 19:    // Explain black mana
                            clickTag = null;
                            break;
				        default:
					        clickTag = "None";
					        clickAll = true;
					        SetArrows(null);
                            manaPayment.pause = false;
                            scrubButton.interactable = true;
                            tutorialPanel.SetActive(false);
                            twoDPanel.SetActive(false);
                            break;
				        }
				    break;
			    case Game.State.GOAL:
				    switch(tutStep){
                        case 10:    // Explain goal movement
                        case 11:    // Explain goal selection display
                        case 22:    // How to win
                        case 23:
                        case 24:
                        case 25:
                            clickTag = null;
                            break;
                        default:
					        clickTag = "None";
					        clickAll = true;
                            tutorialPanel.SetActive(false);
                            twoDPanel.SetActive(false);
                            SetArrows(null);
                            goal.pause = false;
					        break;
				    }
				    break;
			    }
            if (tutStep >= tutText.Length)
            {
                Preferences.Instance.tutorialMode.isOn = false;
                Preferences.Instance.UpdateTutorialMode();
            }
            else if (!clickAll)
            {
                tutorialPanel.SetActive(true);
                twoDPanel.SetActive(true);
                tutorialText.text = tutText[tutStep];
                if (clickTag == null)
                    tutNextButton.gameObject.SetActive(true);
                else
                    tutNextButton.gameObject.SetActive(false);
            }
            }
		    else{
                tutorialPanel.SetActive(false);
                twoDPanel.SetActive(false);
                tutStep = 0;
                scrubButton.interactable = true;
                goal.pause = false;
		    }
	}

	public void ClickAction(Transform hit, string message){
        // Only increment tutorials on click, otherwise release counts double immediately
        if (message == "ClickAction")
        {
            // This is for messages that ask for specific objects to be clicked
            if (hit.transform.tag == clickTag)
            {
                if(!release)
                    tutStep++;
            }
            else if (!clickAll)
                return;
        }
        else if (message == "ReleaseAction" && release) {
            if (hit.transform.tag == clickTag)
                tutStep++;
            else if (!clickAll)
                return;
        }
        hit.SendMessage (message);
	}

    public void IncrementTutStep(string source) {
        if (scrubIncrement && source == "Scrub")
            tutStep++;
        else if (clickTag == null && source == "TutNext")
            tutStep++;
    }
}
