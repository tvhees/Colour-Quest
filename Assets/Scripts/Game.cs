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
            SetUpGame();
        }
    }

}
