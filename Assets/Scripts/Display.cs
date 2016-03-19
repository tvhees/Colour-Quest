using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Display : MonoBehaviour {

	public GameObject displayMana, parent, source;
	public TextMesh[] textArray;
	public Color[] colours;
	public int alignment;

	private float height, width;

	public void Reset(){
		height = Screen.height / 2 / parent.transform.localScale.y;
		width = Screen.width / 10 / parent.transform.localScale.x;

		BuildDisplay ();

		gameObject.SetActive (false);
	}

	private void BuildDisplay(){
		transform.localPosition = new Vector2 (alignment * width/2, height / 2);
		transform.localScale = new Vector2 (width, height);

		float textGap = height / 8;

		for (int i = 0; i < textArray.Length; i++) {
			textArray [i].transform.localScale = new Vector2 (1 / width, 1 / height);
			textArray [i].transform.localPosition = new Vector3 (0f, ((i +1) * textGap - height/2)/height, 0);
			textArray [i].GetComponent<TextMesh>().color = colours [i];
		}
	}

	public void UpdateDisplay(int[] manaList){
		textArray[0].text = manaList [0].ToString ();
		int j = textArray.Length;
		for (int i = 1; i < j; i++) {
				textArray[j-i].text = manaList[i].ToString();
		}
	}
}
