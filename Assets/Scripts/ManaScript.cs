using UnityEngine;
using System.Collections;

public class ManaScript : ClickableObject {

    public int[] value = new int[3] { 0, 0, 0 };
    public Texture wedge;
    public Vector2 size;

    private Rect rect;
    [SerializeField] private bool menu;
 	private GameObject manaHand;
	private ManaPayment manaPayment;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        rect = new Rect(transform.position.x - size.x * 0.5f, transform.position.y - size.y * 0.5f, size.x, size.y);
    }

	public override void OnMouseDown ()
	{
        switch (Game.Instance.state) {
            case Game.State.IDLE:
            case Game.State.ENEMY:
                break;
            case Game.State.PAYING:
                menu = true;
                break;
        }
	}

    private void OnMouseUp() {
        if (menu)
        {
            menu = false;
            AdjustPayment();
        }

    }

    private void AdjustPayment() {
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
    }

    private void OnGUI() {
        if (menu) {
            GUI.DrawTexture(rect, wedge);
        }
    }
}
