using UnityEngine;
using System.Collections;

public class ScrubButton : MonoBehaviour {

	public void ScrubHand(){
		while (Hand.Instance.handMana.Count > 0) {
			Hand.Instance.SendToDiscard (Hand.Instance.handMana [0]);
		}

		Hand.Instance.RefillHand ();
	}
}
