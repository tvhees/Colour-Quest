using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

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
	public DisplayPanel discardDisplay, deckDisplay;
    public enum State
    {
        IDLE,
        PAYING,
        GOAL,
		CAMERA,
        WON,
        LOST
    };
    public State state;

    void Start() {
        SetUpGame();
    }

    public void SetUpGame() {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));

        Master.Instance.game = this;
        Master.Instance.player = player;

        state = State.IDLE;

        if (Master.Instance.newGame)
            boardScript.NewBoard();
        else
            boardScript.InstantiateBoard(false, SaveSystem.Instance.tilesPerRow, SaveSystem.Instance.materials,
                SaveSystem.Instance.flipped, SaveSystem.Instance.goalLocation, SaveSystem.Instance.playerLocation);

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

    public void Show(bool show) {
        gameCamera.enabled = show;
        uiCamera.enabled = show;
        boardCanvas.enabled = show;
        uiCanvas.enabled = show;   
    }

    public void ChangeState(int newState) {
        state = (State)newState;
    }

    void Update()
    {
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

            if (message != null)
                hit.collider.SendMessage(message);
        }
#endif
    }
}
