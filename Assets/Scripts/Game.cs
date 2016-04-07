using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

    public float[] guiBox, guiButton;
    public Text headerText;
    public GameObject menuPanel, prefsPanel, splashPanel, dampener;
    public ParticleSystem menuParticles;
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
    public Camera gameCamera, uiCamera;
    public Canvas boardCanvas, uiCanvas;
    public RaycastHit hit;
    public GUISkin menuSkin;
	public DisplayPanel discardDisplay, deckDisplay;
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
		CAMERA,
        SPLASH
    };
    public State state;

	private State lastState, savedState, savedMenu;
	private string title = null;

    void Start() {
		Preferences.Instance.Load();
        state = State.SPLASH;
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
    }

    public void BackToGame() {
        state = savedState;
    }

    public void BackToMenu() {
        state = savedMenu;
    }

    public void NewGame() {
        Debug.Log("clicked");
        state = State.IDLE;
        SetUpGame();
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
            case State.SPLASH:
                savedMenu = state;
                break;
            default:
                savedState = state;
                break;
        }

        state = (State)newState;
    }

    private void SetMenus(GameObject menu) {
        menuPanel.SetActive(false);
        prefsPanel.SetActive(false);
        splashPanel.SetActive(false);
        dampener.SetActive(false);

        if (menu != null)
        {
            gameCamera.enabled = false;
            boardCanvas.enabled = false;
            uiCamera.enabled = false;
            uiCanvas.enabled = false;

            menu.SetActive(true);
        }
        else {
            gameCamera.enabled = true;
            boardCanvas.enabled = true;
            uiCamera.enabled = true;
            uiCanvas.enabled = true;

            dampener.SetActive(true);
        }
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

        if (state != lastState)
        {
            switch (state)
            {
                case State.MENU:
                    title = "menu";
                    SetMenus(menuPanel);
                    break;
                case State.WON:
                    title = "victory";
                    SetMenus(menuPanel);
                    break;
                case State.LOST:
                    title = "defeat";
                    SetMenus(menuPanel);
                    break;
                case State.PREFS:
                    title = "preferences";
                    SetMenus(prefsPanel);
                    break;
                case State.SPLASH:
                    SetMenus(splashPanel);
                    break;
                default:
                    SetMenus(null);
                    break;
            }
        }

        lastState = state;

        headerText.text = title;

        // Touch detection code
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
			bool touched = false;
			string message = null;

            switch (touch.phase)
            {
                // If touch is beginning, get the collider being touched
                case TouchPhase.Began:
                    message = "ClickAction";
                    touched = Physics.Raycast(uiCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit);
                    if (!touched)
                        touched = Physics.Raycast(gameCamera.ScreenPointToRay(Input.GetTouch(0).position), out hit);
                    break;
                // If touch is ending, tell the collider that was originally touched
                case TouchPhase.Ended:
                    if (hit.transform != null)
                    {
                        message = "ReleaseAction";
                    }
                    break;
            }
        }
#endif
    }
}
