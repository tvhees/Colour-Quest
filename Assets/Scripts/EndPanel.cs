using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndPanel : MonoBehaviour {
    public Text header;
    public Text rank;
    public Button newGame, menu;

    public IEnumerator GameEnd(bool victory) {
        newGame.interactable = false;
        menu.interactable = false;

        if (victory)
        {
            header.text = "victory";
            Preferences.Instance.UpdateDifficulty(1);
            rank.text = "new rank: " + Preferences.Instance.difficulty;
        }
        else
        {
            header.text = "defeat";
            Preferences.Instance.UpdateDifficulty(-1);
            rank.text = "new rank: " + Preferences.Instance.difficulty;
        }

        yield return new WaitForSeconds(0.1f);

        newGame.interactable = true;
        menu.interactable = true;
    }
}
