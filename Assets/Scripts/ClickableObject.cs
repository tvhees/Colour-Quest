using UnityEngine;
using System.Collections;

public abstract class ClickableObject : MonoBehaviour {

#if UNITY_STANDALONE || UNITY_EDITOR
    public void OnMouseDown() {
		// If the tutorial is running we want extra control over what happens
		if(Preferences.Instance.tutorial){
			if(Game.Instance.state == Game.State.IDLE || Game.Instance.state == Game.State.PAYING || Game.Instance.state == Game.State.GOAL){
				Game.Instance.tutorial.ClickAction(transform, "ClickAction");
			}
		}
		else 
	        ClickAction();
    }
#endif

    public abstract void ClickAction();

    public virtual void KillTile(bool dead) {

    }
}
