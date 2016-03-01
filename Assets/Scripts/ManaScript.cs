﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManaScript : ClickableObject {

    public int[] value = new int[3] { 0, 0, 0 }, savedValue;
    public float menuDelay = 0.1f;
    public ParticleSystem selectFX;

	private List<GameObject> options = new List<GameObject>(), blackMana = new List<GameObject>();
    private Vector3 wedgeOffset = new Vector3(0f, 0f, 0f), rotation = Vector3.up;
    private float clickTime;
    private bool menu;
	private Material savedMaterial;
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
				if(HandManager.Instance.handMana.Contains(gameObject)){
					if (!HandManager.Instance.blackMana.Contains (gameObject)) {
						clickTime = Time.time;
						menu = true;
					}
				}
                break;
        }
	}

    private void OnMouseUp() {
             
		if (menu) {
			menu = false;

			// Check if we are over the main mana globe or the option wedges. Do nothing if not.
			RaycastHit hit;
			Ray ray = uiCamera.ScreenPointToRay (Input.mousePosition);
			Physics.Raycast (ray.origin, ray.direction, out hit);
			wedge.SetActive (false);

			if (hit.collider == GetComponent<Collider> ())
				Select (HandManager.Instance.selectedMana.Contains (gameObject));

			if (hit.collider == wedgeLower.GetComponent<Collider> () || hit.collider == wedgeUpper.GetComponent<Collider> ()) {
				UseOption (hit.collider.gameObject);
				Select (HandManager.Instance.selectedMana.Contains (gameObject));
			}
		
		}

		// Remove any UI mana globes and put the object back in line with the rest.
		if (options.Count > 0)
		{
            while (options.Count > 0)
            {
				HandManager.Instance.SendToPool(options[0]);
                options.Remove(options[0]);
            }
			transform.position = transform.position + Vector3.forward;
		}
    }

	private void Select(bool selected){
		if (!selected) { // Newly selected mana - recalculate mana payment and show particles
			HandManager.Instance.selectedMana.Add (gameObject);
			selectFX.Play ();
			manaPayment.CheckPayment (value, true);
		} else { // Deselected mana - recalculate mana payment, remove any added black mana, reset colour/particles
			HandManager.Instance.selectedMana.Remove (gameObject);
			manaPayment.CheckPayment (value, false);
            while (blackMana.Count > 0) {
				HandManager.Instance.SendToPool(blackMana[0]);
                blackMana.Remove(blackMana[0]);
            }

			Reset();
		}
	}

	public void Reset(){
		// Return to original colour, turn off particles, clear associations with black mana generated by this
		selectFX.Stop();
		value = savedValue;
		GetComponent<MeshRenderer> ().material = savedMaterial;
        blackMana.Clear();
	}

    void Update() {
        //transform.Rotate(rotation, 80 * Time.deltaTime);
        
		if (options.Count == 0){
			if (menu && (Time.time - clickTime) > menuDelay && !HandManager.Instance.selectedMana.Contains(gameObject)) 
            {
                transform.position = transform.position - Vector3.forward;
                SpawnOptions();

                wedge.transform.position = transform.position + wedgeOffset;
                wedge.SetActive(true);
            }
        }
    }

    private void SpawnOptions() {
		// Next colour along + 1 black mana
        AlternateMana(new Vector3(7f, 30f, 0f), value, 1);
        AlternateMana(new Vector3(7f, -10f, 0f), new int[3] {0, 0, 0}, 1);

		// Two colours along + 2 black mana
        AlternateMana(new Vector3(7f, 10f, 0f), value, 2);
        AlternateMana(new Vector3(-8f, -30, 0f), new int[3] { 0, 0, 0 }, 2);
        AlternateMana(new Vector3(22f, -30f, 0f), new int[3] { 0, 0, 0 }, 2);
    }

    private void AlternateMana(Vector3 position, int[] manaValue, int blackNumber) {
        GameObject option = manaPool.GetManaOption(manaValue, blackNumber);

        // Optional mana is smaller to fit in selection wedges
        option.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        option.transform.SetParent(wedge.transform.GetChild(blackNumber - 1));
        option.transform.localPosition = position;
        option.SetActive(true);
        // Track optional mana so we can send it back to the pool later
        options.Add(option);
    }

    private void UseOption(GameObject wedgeHalf) {
		// Save original value and material
        savedValue = value;
        savedMaterial = GetComponent<MeshRenderer> ().material;
        // Copy new value and material
        GameObject newMana = wedgeHalf.transform.GetChild(0).gameObject;
        value = newMana.GetComponent<ManaScript>().value;
        GetComponent<MeshRenderer>().material = newMana.GetComponent<MeshRenderer>().material;

		// Get any black mana required for this option and add it to player's hand
		while(wedgeHalf.transform.childCount > 1){
            Debug.Log(wedgeHalf.transform.childCount);
			newMana = wedgeHalf.transform.GetChild (1).gameObject;
			HandManager.Instance.SendToHand (newMana);
			options.Remove (newMana);
			blackMana.Add (newMana);
			HandManager.Instance.blackMana.Add (newMana);
		}
    }

	public void SaveState(){
		savedValue = value;
		savedMaterial = GetComponent<MeshRenderer> ().material;
	}
}
