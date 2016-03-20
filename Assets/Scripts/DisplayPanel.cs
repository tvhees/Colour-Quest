using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPanel : MonoBehaviour {

	public GameObject displayMana, parent, source;
	public TextMesh[] textArray;
	public Color[] colours;
	public int alignment;

	private float height, width;

	public void Reset(){

		float heightScale = 1f;
		float widthScale = 1f;

		#if UNITY_ANDROID && !UNITY_EDITOR
		heightScale = 900f / Screen.currentResolution.height;
		widthScale = 600f / Screen.currentResolution.width;
		#endif

		height = heightScale * Screen.height / parent.transform.localScale.y / 200f;
		width = widthScale * Screen.width / parent.transform.localScale.x / 1000f;

		BuildDisplay ();

		gameObject.SetActive (false);
	}

	private void BuildDisplay(){
		transform.localPosition = new Vector2 (alignment * width/2f, height / 2f);
		transform.localScale = new Vector2 (width, height);

		float textGap = height / 8f;

		for (int i = 0; i < textArray.Length; i++) {
			textArray [i].transform.localScale = new Vector2 (1f / width, 1f / height);
			textArray [i].transform.localPosition = new Vector3 (0f, ((float)(i +1) * textGap - height/2f)/height, 0f);
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
