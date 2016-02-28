using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaScript : ClickableObject {

    public int[] value = new int[3] { 0, 0, 0 };
    public float menuDelay = 0.1f;
    public ParticleSystem selectFX;

    private List<GameObject> options = new List<GameObject>();
    private Vector3 wedgeOffset = new Vector3(0f, 0f, 0f), rotation = Vector3.up;
    private float clickTime;
    private bool menu;
 	private GameObject manaHand, wedge, wedgeUpper, wedgeLower;
	private ManaPayment manaPayment;
    private ManaPool manaPool;
    private Camera uiCamera;

	void Awake(){
		manaHand = GameObject.Find ("ManaHand");
        wedge = GameObject.Find("Wedge");
        wedgeUpper = wedge.transform.GetChild(1).gameObject;
        wedgeLower = wedge.transform.GetChild(0).gameObject;
		manaPayment = manaHand.GetComponent<ManaPayment> ();
        manaPool = manaHand.GetComponent<ManaPool>();
        uiCamera = manaHand.transform.parent.GetComponent<Camera>();
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



            // Check if we are over the main mana globe
            RaycastHit hit;
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray.origin, ray.direction, out hit);
            wedge.SetActive(false);

            Debug.Log(hit.transform.gameObject.ToString());

            if (hit.collider == GetComponent<Collider>())
                AdjustPayment();
            else if (hit.collider == wedgeLower.GetComponent<Collider>() || hit.collider == wedgeUpper.GetComponent<Collider>()){
                UseOption(hit.collider.gameObject);
                AdjustPayment();                
            }
        }

        if (options.Count > 0)
        {
            while (options.Count > 0)
            {
                manaPool.SendToPool(options[0]);
                options.Remove(options[0]);
            }
            transform.position = transform.position + Vector3.forward;
        }
    }

    private void AdjustPayment() {
         if (HandManager.Instance.selectedMana.Contains(gameObject))
                        {
                            HandManager.Instance.selectedMana.Remove(gameObject);
                            manaPayment.CheckPayment(value, false);
                            selectFX.Stop();
                        }
                        else
                        {
                            HandManager.Instance.selectedMana.Add(gameObject);
                            manaPayment.CheckPayment(value, true);
                            selectFX.Play();
                        }
    }

    void Update() {
        //transform.Rotate(rotation, 80 * Time.deltaTime);
        
        if (menu && (Time.time - clickTime) > menuDelay) {
            if (options.Count == 0)
            {
                transform.position = transform.position - Vector3.forward;
                SpawnOptions();

                wedge.transform.position = transform.position + wedgeOffset;
                wedge.SetActive(true);
            }
        }
    }

    private void SpawnOptions() {
        AlternateMana(new Vector3(7f, 30f, 0f), value, 1);
        AlternateMana(new Vector3(7f, -10f, 0f), new int[3] {0, 0, 0}, 1);
        AlternateMana(new Vector3(7f, 10f, 0f), value, 2);
        AlternateMana(new Vector3(-8f, -30, 0f), new int[3] { 0, 0, 0 }, 2);
        AlternateMana(new Vector3(22f, -30f, 0f), new int[3] { 0, 0, 0 }, 2);
    }

    private void AlternateMana(Vector3 position, int[] manaValue, int blackMana) {
        GameObject option = manaPool.GetManaOption(manaValue, blackMana);
        option.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        option.transform.SetParent(wedge.transform.GetChild(blackMana - 1));
        option.transform.localPosition = position;
        option.SetActive(true);
        options.Add(option);
    }

    private void UseOption(GameObject wedgeHalf) {
        GameObject newMana = wedgeHalf.transform.GetChild(0).gameObject;
        value = newMana.GetComponent<ManaScript>().value;
        GetComponent<MeshRenderer>().material = newMana.GetComponent<MeshRenderer>().material;
    }
}
