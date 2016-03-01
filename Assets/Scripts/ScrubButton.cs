using UnityEngine;
using System.Collections;

public class ScrubButton : MonoBehaviour {

	public void ScrubHand(){
		while (HandManager.Instance.handMana.Count > 0) {
			HandManager.Instance.SendToDiscard (HandManager.Instance.handMana [0]);
		}

		HandManager.Instance.RefillHand ();
	}
}
