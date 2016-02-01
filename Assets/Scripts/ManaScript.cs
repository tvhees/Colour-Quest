using UnityEngine;
using System.Collections;

public class ManaScript : ClickableObject {

	private bool clicked;
	private GameObject manaHand;
	private ManaPayment manaPayment;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();

		clicked = false;
	}

	public override void MouseClick ()
	{
		if (clicked) {
			manaPayment.CheckPayment (new int[3]{ -1, 0, 0 });
		} else {
			manaPayment.CheckPayment (new int[3]{1, 0, 0});
		}

		clicked = !clicked;
	}
}
