using UnityEngine;
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

    public State appState, savedState;
    public GameObject[] menuPanels;

    void Start()
    {
        // Every time app opens we want to get any saved preferences first
        // And then open the title screen
        Preferences.Instance.Load();
        appState = State.TITLE;
    }

    public void ChangeState(int newState)
    {

        // Restore the previous state if called with a negative int
        // Otherwise store the current state and get the new one.
        if (newState < 0)
            appState = savedState;
        else {
            savedState = appState;
            appState = (State)newState;
        }

        // Either way update panels to reflect current state
        SetMenus(menuPanels[(int)appState]);
    }

    private void SetMenus(GameObject menu)
    {
        // Turn off all menu panels except the desired one.
        // Position in the array matches enum integer.
        // For "Game" state we turn on the backgroundDampener panel
        for (int i = 0; i < menuPanels.Length; i++)
            menuPanels[i].SetActive(menuPanels[i] == menu);
    }
}
