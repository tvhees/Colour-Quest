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

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (state == State.MENU)
            {
                state = savedState;
            }
            else {
                savedState = state;
                state = State.MENU;
            }
        }
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
