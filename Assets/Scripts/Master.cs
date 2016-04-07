using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Master : Singleton<Master>
{

    public enum State
    {
        TITLE,
        MENU,
        PREFS,
        GAME
    }

    public State state, savedState;
    public GameObject[] menuPanels;
    public Game game;
    public Player player;
    public Hand hand;
    public bool newGame;

    void Start()
    {
        // Every time app opens we want to get any saved preferences first
        // And then open the title screen
        Preferences.Instance.Load();
        state = State.TITLE;
    }

    public void StartGame(bool continuing)
    {
        // We need a flag to tell the game manager whether to make a new board entirely
        newGame = !continuing;

        ChangeState((int)State.GAME);

        // Unload any existing game scene, then load a new one
        if (SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.UnloadScene("Game");

        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }

    public void ChangeState(int newState)
    {

        // Restore the previous state if called with a negative int
        // Otherwise store the current state and get the new one.
        if (newState < 0)
            state = savedState;
        else {
            savedState = state;
            state = (State)newState;
        }

        // Either way update panels to reflect current state
        SetMenus(menuPanels[(int)state]);

        // Show or hide the game appropriately
        // If we've entered the GAME state this enables cameras and canvasses
        if (game != null)
            game.Show(state == State.GAME);
    }

    private void SetMenus(GameObject menu)
    {
        // Turn off all menu panels except the desired one.
        // Position in the array matches enum integer.
        // For "Game" state we turn on the backgroundDampener panel
        for (int i = 0; i < menuPanels.Length; i++)
            menuPanels[i].SetActive(menuPanels[i] == menu);
    }

    void Update() {
        // Escape key used to get to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == State.MENU)
            {
                ChangeState((int)State.GAME);
            }
            else if (state == State.PREFS)
            {
                Preferences.Instance.Save();
                // Return to whichever menu we were last in
                ChangeState(-1);
            }
            else if (state == State.GAME)
            {
                ChangeState((int)State.MENU);
            }
        }
    }

    public void QuitApp() {
        Debug.Log("Exiting the app");
        Application.Quit();
    }
}
