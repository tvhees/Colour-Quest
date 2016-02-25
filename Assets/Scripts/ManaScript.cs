using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManaScript : ClickableObject {

    public int[] value = new int[3] { 0, 0, 0 };
    public float menuDelay = 0.1f;

    private List<GameObject> options = new List<GameObject>();
    private Vector3 wedgeOffset = new Vector3(0f, 0f, 0f);
    private float clickTime;
    private bool menu;
 	private GameObject manaHand, wedge;
	private ManaPayment manaPayment;
    private ManaPool manaPool;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
        wedge = GameObject.Find("Wedge");
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        manaPool = manaHand.GetComponent<ManaPool>();
    }

	public override void OnMouseDown ()
	{
        switch (Game.Instance.state) {
            case Game.State.IDLE:
            case Game.State.ENEMY:
                break;
            case Game.State.PAYING:
                clickTime = Time.time;
                menu = true;
                break;
        }
	}

    private void OnMouseUp() {
        if (menu)
        {
            menu = false;
            wedge.SetActive(false);
            AdjustPayment();
        }

       while(options.Count > 0)
        {
           manaPool.SendToPool(options[0]);
           options.Remove(options[0]);
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

    void Update() {
        if (menu && (Time.time - clickTime) > menuDelay) {
            wedge.transform.position = transform.position + wedgeOffset;
            wedge.SetActive(true);
            if (options.Count == 0)
                SpawnOptions();
        }
    }

    private void SpawnOptions() {
        AlternateMana(new Vector3(1.35f, -0.5f, 0f), value, 1);
        AlternateMana(new Vector3(1.35f, -1.35f, 0f), new int[3] {0, 0, 0}, 1);
        AlternateMana(new Vector3(1.35f, 1.35f, 0f), value, 2);
        AlternateMana(new Vector3(1.00f, 0.5f, 0f), new int[3] { 0, 0, 0 }, 1);
        AlternateMana(new Vector3(1.70f, 0.5f, 0f), new int[3] { 0, 0, 0 }, 1);
    }

    private void AlternateMana(Vector3 position, int[] manaValue, int blackMana) {
        GameObject option = manaPool.GetManaOption(manaValue, blackMana);
        option.transform.SetParent(transform);
        option.transform.localPosition = position;
        option.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        option.SetActive(true);
        options.Add(option);
    }
}
