using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

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

    public enum State
    {
        IDLE,
        PAYING,
        GOAL,
        MENU
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.MENU)
            {
                state = savedState;
            }
            else
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
        if (state == State.MENU) {
            GUI.Box(new Rect(150, 200, 300, 200), "Pause Menu");

            // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
            if (GUI.Button(new Rect(170, 240, 260, 70), "Restart Game"))
            {
                SetUpGame();
            }

            // Make the second button.
            if (GUI.Button(new Rect(170, 320, 260, 70), "Quit Game"))
            {
                Application.Quit();
            }
        }
    }

}
