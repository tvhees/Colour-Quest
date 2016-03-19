using UnityEngine;
using System.Collections.Generic;

public abstract class Collection<T> : Singleton<T> where T : MonoBehaviour
{
    public float objScale, gapScale;
    public List<GameObject> contents, blackMana;
    public ObjectPool pool;
	public bool centered;
	public int[] manaList;

    protected int size;
	protected Vector3 translate;

    public virtual void Reset() {
		SharedSetup ();
    }

	protected virtual void SharedSetup(){
		manaList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
		centered = true;

		while (contents.Count > 0)
		{
			pool.SendToPool(contents[0]);
		}

		translate = new Vector3 (0.5f * objScale * gapScale, 0f, 0f);
	}

    public void AddObj(GameObject obj) {
        // Add to container
        contents.Add(obj);
		if (obj.GetComponent<Mana> () != null) {
			int[] value = obj.GetComponent<Mana> ().value;
			if (value.Sum () == 0)
				blackMana.Add (obj);
			manaList [pool.GetValueIndex (value)]++;
		}
        // Track mana in container
        size++;
		
        // Assign local position to the right of existing mana
        obj.transform.parent = transform;
        obj.transform.localScale = objScale * Vector3.one;

		Vector3 localPoint = FindPosition (size, new Vector3(0f, 0f, 0f));

		obj.transform.localPosition = localPoint;

		if (centered) {
			// Shift all mana leftward to keep the hand centered
			for (int i = 0; i < contents.Count; i++) {
				contents [i].transform.localPosition = FindPosition (-1, contents [i].transform.localPosition);
			}
		}

	}

    public void Remove(GameObject obj) {
        if (contents.Contains(obj)) {
            int j = contents.IndexOf(obj);
            contents.Remove(obj);
            size--;

			if (obj.GetComponent<Mana> () != null) {
				int[] value = obj.GetComponent<Mana> ().value;
				manaList [pool.GetValueIndex (value)]--;
			}

			for (int i = 0; i < contents.Count; i++) {
				if (i < j)
					contents [i].transform.localPosition = FindPosition(1, contents [i].transform.localPosition);
				else 
					contents [i].transform.localPosition = FindPosition(-1, contents [i].transform.localPosition);
			}
        }

        if (blackMana.Contains(obj)) {
            blackMana.Remove(obj);
        }
    }

	protected virtual Vector3 FindPosition(int scalar, Vector3 pos){
		Vector3 position = pos + scalar * translate;
		return position;
	}
}