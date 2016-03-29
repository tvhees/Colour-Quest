using UnityEngine;
using System.Collections;

public class PayButton : MonoBehaviour {

    public ManaPayment manaPayment;

#if UNITY_STANDALONE || UNITY_EDITOR
    void OnMouseDown() {
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

    void ClickAction() {
        if (manaPayment.payed)
			StartCoroutine(manaPayment.ConfirmPayment());
    }
}
