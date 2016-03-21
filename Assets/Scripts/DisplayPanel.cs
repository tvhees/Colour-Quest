using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPanel : MonoBehaviour {

	public GameObject displayMana, parent, source, uiCamera, player;
	public TextMesh[] textArray;
	public Color[] colours;
	public float alignment, height, width;

	public void Reset(){

		height = player.transform.localScale.x;
		width = 4f * player.transform.localScale.y;
		transform.SetParent (player.transform);

		BuildDisplay ();

		gameObject.SetActive (false);
	}

	private void BuildDisplay(){
		transform.localPosition = new Vector2 (0f, 1.5f*height);
		transform.localScale = new Vector2 (width, height);

		for (int i = 0; i < textArray.Length; i++) {
			textArray [i].transform.localScale = new Vector2 (1f / width, 1f / height);
			textArray [i].transform.localPosition = new Vector3 ((-0.5f * (textArray.Length - 1) + i)/(textArray.Length+1f), 0f, 0f);
			textArray [i].GetComponent<TextMesh>().color = colours [i];
		}
	}

	public void UpdateDisplay(int[] manaList){
		textArray[0].text = manaList [0].ToString ();
		int j = textArray.Length;
		for (int i = 1; i < j; i++) {
				textArray[i].text = manaList[i].ToString();
		}
	}
}
