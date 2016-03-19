using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

    public float[] guiBox, guiButton;
    public ManaPayment manaPayment;
    public BoardScript boardScript;
    public Player player;
    public Goal goal;
    public ObjectivePool objectivePool;
    public Hand hand;
    public Deck deck;
    public Discard discard;
    public ManaPool manaPool;
    public Camera mainCamera;
    public Camera uiCamera;
    public RaycastHit hit;
    public GUISkin menuSkin;
	public Display discardDisplay;
	public Display deckDisplay;

    public enum State
    {
        IDLE,
        PAYING,
        GOAL,
        MENU,
        WON,
        LOST
    };

    public State state;

    private State savedState;

    void Start() {
        SetUpGame();
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
        manaPool.Reset();
		discardDisplay.Reset ();
		deckDisplay.Reset ();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.MENU)
            {
                state = savedState;
            }
            else if(state != State.WON || state != State.LOST)
            {
                savedState = state;
                state = State.MENU;
            }
        }

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

        if (state == State.MENU) {
            GUI.Box(new Rect(Screen.width * guiBox[0], Screen.height*guiBox[1], Screen.width * guiBox[2], Screen.height * guiBox[3]), "MENU");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * guiButton[1], Screen.width * guiButton[2], Screen.height * guiButton[3]), "RESTART"))
            {
                SetUpGame();
            }

            // Make the second button.
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * (guiButton[1] + 1.1f*guiButton[3]), Screen.width * guiButton[2], Screen.height * guiButton[3]), "QUIT"))
            {
                Application.Quit();
            }
        }

        if (state == State.WON) {
            GUI.Box(new Rect(Screen.width * guiBox[0], Screen.height * guiBox[1], Screen.width * guiBox[2], Screen.height * guiBox[3]), "VICTORY");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * guiButton[1], Screen.width * guiButton[2], Screen.height * guiButton[3]), "RESTART"))
            {
                SetUpGame();
            }

            // Make the second button.
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * (guiButton[1] + 1.1f * guiButton[3]), Screen.width * guiButton[2], Screen.height * guiButton[3]), "QUIT"))
            {
                Application.Quit();
            }
        }

        if (state == State.LOST) {
            GUI.Box(new Rect(Screen.width * guiBox[0], Screen.height * guiBox[1], Screen.width * guiBox[2], Screen.height * guiBox[3]), "DEFEAT");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * guiButton[1], Screen.width * guiButton[2], Screen.height * guiButton[3]), "RESTART"))
            {
                SetUpGame();
            }

            // Make the second button.
            if (GUI.Button(new Rect(Screen.width * guiButton[0], Screen.height * (guiButton[1] + 1.1f * guiButton[3]), Screen.width * guiButton[2], Screen.height * guiButton[3]), "QUIT"))
            {
                Application.Quit();
            }
        }
    }

}
