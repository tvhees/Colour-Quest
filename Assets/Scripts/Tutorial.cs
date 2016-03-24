using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {

	public GameObject[] arrows;

	private List<GameObject> offArrows = new List<GameObject>();

	void Update () {
		// Tutorial arrow management
		if(Preferences.Instance.tutorial){
			switch (Game.Instance.state) {
			case Game.State.IDLE:
				SetArrows (new int[1]{0});
				break;
			case Game.State.PAYING:
				SetArrows (new int[3]{2, 4, 5});
				break;
			case Game.State.GOAL:
				SetArrows (new int[1]{ 1 });
				break;
			}
		}
		else{
			for(int i = 0; i < arrows.Length; i++){
				arrows[i].SetActive(false);
			}
		}
	}

	private void SetArrows(int[] index){
		offArrows.Clear();
		offArrows.AddRange (arrows);
		for (int i = 0; i < index.Length; i++) {
			arrows [index [i]].SetActive (true);
			offArrows.Remove (arrows [index [i]]);
		}

		for (int i = 0; i < offArrows.Count; i++) {
			offArrows [i].SetActive (false);
		}
	}

}
