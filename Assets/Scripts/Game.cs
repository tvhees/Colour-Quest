using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {

    public enum State
    {
        IDLE,
        PAYING,
        GOAL
    };

    public State state;

    void Start() {
        state = State.IDLE;
    }

}
