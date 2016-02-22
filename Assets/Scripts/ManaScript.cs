using UnityEngine;
using System.Collections;

public class ManaScript : ClickableObject {

    public int[] value = new int[3] { 0, 0, 0 };

 	private GameObject manaHand;
	private ManaPayment manaPayment;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
	}

	public override void MouseClick ()
	{
        switch (Game.Instance.state) {
            case Game.State.IDLE:
            case Game.State.ENEMY:
                break;
            case Game.State.PAYING:
                if (HandManager.Instance.selectedMana.Contains(gameObject))
                {
                    HandManager.Instance.selectedMana.Remove(gameObject);
                    manaPayment.CheckPayment(value, false);
                }
                else
                {
                    HandManager.Instance.selectedMana.Add(gameObject);
                    manaPayment.CheckPayment(value, true);
                }
                break;
        }
	}
}
