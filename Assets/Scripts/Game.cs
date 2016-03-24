using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

    public float[] guiBox, guiButton;
	public RectTransform uiRect;
    public ManaPayment manaPayment;
    public BoardScript boardScript;
    public Player player;
    public Goal goal;
    public ObjectivePool objectivePool;
    public Hand hand;
    public Deck deck;
	public Preview preview;
    public Discard discard;
    public ManaPool manaPool;
    public Camera mainCamera;
    public Camera uiCamera;
    public RaycastHit hit;
    public GUISkin menuSkin;
	public DisplayPanel discardDisplay;
	public DisplayPanel deckDisplay;
	public float uiGap;
    public enum State
    {
        IDLE,
        PAYING,
        GOAL,
        MENU,
		PREFS,
        WON,
        LOST
    };
    public State state;

	private State savedState, savedMenu;
	private string title = null;
	private float screenWidth, screenHeight;

    void Start() {
		Preferences.Instance.Load();
        SetUpGame();

		// Get dimensions to use for menus
		screenWidth = Screen.width; //uiRect.rect.width;
		screenHeight = Screen.height; //uiRect.rect.height;
    }

    void SetUpGame() {
		
        boardScript.NewBoard();
        manaPayment.Reset();
        player.Reset();
        goal.Reset();
        objectivePool.Reset();
        hand.Reset();
        deck.Reset();
        discard.Reset();
		preview.Reset();
        manaPool.Reset();
		discardDisplay.Reset ();
		deckDisplay.Reset ();
    }

    void Update()
    {

		// Escape key used to get to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.MENU)
			{
                state = savedState;
            }
			else if (state == State.PREFS) {
				state = savedMenu;
			}
            else if(state != State.WON || state != State.LOST)
            {
                savedState = state;
                state = State.MENU;
            }
        }

		// Touch detection code
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase) {
                case TouchPhase.Began:
                    if (Physics.Raycast(uiCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                        hit.transform.SendMessage("ClickAction");
                    else if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit))
                        hit.transform.SendMessage("ClickAction");
                    break;
                case TouchPhase.Ended:
                    if (hit.transform != null) {
                        hit.transform.SendMessage("ReleaseAction");
                    }
                    break;
            }
        }
#endif
    }


    void OnGUI() {
        GUI.skin = menuSkin;

		switch (state) {
		case State.MENU:
			title = "MENU";
			break;
		case State.WON:
			title = "VICTORY";
			break;
		case State.LOST:
			title = "DEFEAT";
			break;
		case State.PREFS:
			title = "PREFERENCES";
			break;
		}

		// Main menu - on Escape or win/loss
		if (state == State.MENU || state == State.WON || state == State.LOST) {
			
            GUI.Box(new Rect(screenWidth * guiBox[0], screenHeight*guiBox[1], screenWidth * guiBox[2], screenHeight * guiBox[3]), title);

            // Make the first button. If it is pressed, a new game will be set up
			if (GUI.Button(MakeUIRect(0f * uiGap), "RESTART"))
            {
                SetUpGame();
            }

            // Make the second button. Go to the preferences menu
			if (GUI.Button(MakeUIRect(1f * uiGap), "PREFERENCES"))
            {
				savedMenu = state;
				state = State.PREFS;
            }

			// Make the third button. Returns to the game
			if (GUI.Button(MakeUIRect(2f * uiGap), "BACK"))
			{
				state = savedState;
			}

			// Make the fourth button. Quit the game
			if (GUI.Button(MakeUIRect(3f * uiGap), "QUIT"))
			{
				Application.Quit();
			}
        }

		// Preferences menu - on pressing preferences button
		if (state == State.PREFS) {
			GUI.Box(new Rect(screenWidth * guiBox[0], screenHeight*guiBox[1], screenWidth * guiBox[2], screenHeight * guiBox[3]), title);

			// Make the first button. Turns tutorial features on/off
			Preferences.Instance.tutorial = GUI.Toggle(MakeUIRect(0f * uiGap), Preferences.Instance.tutorial, "TUTORIAL MODE");


			// Make a toggle that turns goal following with the camera on/off
			Preferences.Instance.watchGoal = GUI.Toggle(MakeUIRect(1f * uiGap), Preferences.Instance.watchGoal, "WATCH GOAL");

			// Make a slider. Changes speed of camera pan and zoom
			Preferences.Instance.cameraSpeed = GUI.HorizontalSlider(MakeUIRect(2f * uiGap), Preferences.Instance.cameraSpeed, 3f, 10f);
		
			// Make a label for the slider
			GUI.Label(MakeUIRect(2f * uiGap),"CAMERA SPEED");

			// Make the third button. Returns to previous Menu
			if (GUI.Button(MakeUIRect(3f * uiGap), "RESET"))
			{
				PlayerPrefs.DeleteAll();
				Preferences.Instance.Load();
			}

			// Make the fourth button. Returns to previous Menu
			if (GUI.Button(MakeUIRect(4f * uiGap), "BACK"))
			{
				state = savedMenu;
			}
		}
    }
	private Rect MakeUIRect(float multiplier){
		return new Rect (screenWidth * guiButton [0], screenHeight * (guiButton [1] + multiplier * guiButton [3]),
						screenWidth * guiButton [2], screenHeight * guiButton [3]);
	}
}
