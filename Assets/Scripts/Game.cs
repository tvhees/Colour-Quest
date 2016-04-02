using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

    public float[] guiBox, guiButton;
    public Text headerText;
    public GameObject menuPanel, prefsPanel;
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
	public DisplayPanel discardDisplay, deckDisplay;
	public Tutorial tutorial;
	public float uiGap;
    public enum State
    {
        IDLE,
        PAYING,
        GOAL,
        MENU,
		PREFS,
        WON,
        LOST,
		CAMERA
    };
    public State state;

	private State savedState, savedMenu;
	private string title = null;

    void Start() {
		Preferences.Instance.Load();
        SetUpGame();
    }

    public void SetUpGame() {
        boardScript.NewBoard();
        player.Reset();
        goal.Reset();
		manaPayment.Reset();
        objectivePool.Reset();
        hand.Reset();
        deck.Reset();
        discard.Reset();
		preview.Reset();
        StartCoroutine(manaPool.Reset());
		discardDisplay.Reset ();
		deckDisplay.Reset ();
		tutorial.Reset ();
    }

    public void BackToGame() {
        state = savedState;
    }

    public void BackToMenu() {
        state = savedMenu;
    }

    public void Quit() {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void ChangeState(int newState) {
        switch (state) {
            case State.MENU:
            case State.WON:
            case State.LOST:
            case State.PREFS:
                savedMenu = state;
                break;
            default:
                savedState = state;
                break;
        }

        state = (State)newState;
    }

    void Update()
    {

		// Escape key used to get to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.MENU)
			{
                BackToGame();
            }
			else if (state == State.PREFS) {
				Preferences.Instance.Save ();
                BackToMenu();
			}
            else if(state != State.WON || state != State.LOST)
            {
                ChangeState((int)State.MENU);
            }
        }

        menuPanel.SetActive(false);
        prefsPanel.SetActive(false);

        switch (state)
        {
            case State.MENU:
                title = "MENU";
                menuPanel.SetActive(true);
                break;
            case State.WON:
                title = "VICTORY";
                menuPanel.SetActive(true);
                break;
            case State.LOST:
                title = "DEFEAT";
                menuPanel.SetActive(true);
                break;
            case State.PREFS:
                title = "PREFERENCES";
                prefsPanel.SetActive(true);
                break;
        }

        headerText.text = title;

        // Touch detection code
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
			bool touched = false;
			string message = null;

            switch (touch.phase) {
			// If touch is beginning, get the collider being touched
                case TouchPhase.Began:
					message = "ClickAction";
					touched = Physics.Raycast(uiCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit);
					if(!touched)
						touched = Physics.Raycast(mainCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit);
					break;
			// If touch is ending, tell the collider that was originally touched
                case TouchPhase.Ended:
                    if (hit.transform != null) {
					message = "ReleaseAction";
                    }
                    break;
            }

			// If the tutorial is running we want extra control over what happens
			if(Preferences.Instance.tutorial){
				if(state == State.IDLE || state == State.PAYING || state == State.GOAL){
					tutorial.ClickAction(hit.transform, message);
				}
			}
			else if(message != null)
				hit.transform.SendMessage(message);
        }
#endif
    }
}
