using UnityEngine;
using System.Collections.Generic;

public abstract class Collection<T> : Singleton<T> where T : MonoBehaviour
{
    public float objScale, gapScale;
    public List<GameObject> contents, blackMana, hidden, preview;
    public ObjectPool pool;
	public bool centered, valueOnAdd, valueOnRemove;
	public int[] manaList;

    protected int size;
	protected Vector3 translate;

    public virtual void Reset() {
		valueOnAdd = true;
		valueOnRemove = true;
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

    public virtual void AddObj(GameObject obj) {
        // Add to container
        contents.Add(obj);
		if (obj.GetComponent<Mana> () != null && valueOnAdd) {
			ChangeValue (obj, true);
		}
        // Track mana in container
        size++;
		
        // Assign local position to the right of existing mana
        obj.transform.parent = transform;
        obj.transform.localScale = objScale * Vector3.one;

		Vector3 localPoint;

		if (centered) {
			localPoint = FindPosition (-1f * size * Mathf.Pow(-1, size), new Vector3(0f, 0f, 0f));
			obj.transform.localPosition = localPoint;

			// Shift all mana to keep the hand centered
			for (int i = 0; i < size; i++) {
				contents [i].transform.localPosition = FindPosition (Mathf.Pow(-1,size), contents [i].transform.localPosition);
			}
		} else {
			localPoint = FindPosition (size, new Vector3(0f, 0f, 0f));

			obj.transform.localPosition = localPoint;
		}

	}

	protected virtual void ChangeValue(GameObject obj, bool increase){
		int[] value = obj.GetComponent<Mana> ().value;
		if (increase) {
			if (value.Sum () == 0)
				blackMana.Add (obj);
			manaList [pool.GetValueIndex (value)]++;
		}
		else {
			manaList [pool.GetValueIndex (value)]--;
		}
	}

    public virtual void Remove(GameObject obj) {
        if (contents.Contains(obj)) {
			float j = obj.transform.position.x;
            contents.Remove(obj);
            size--;

			if (obj.GetComponent<Mana> () != null && valueOnRemove) {
				ChangeValue (obj, false);
			}

			int direction = 1;

			// Shift objects towards position of the removed object *** THIS CODE IS PROBABLY CAUSING ISSUES
			if (centered) {
				for(int i = 0; i < size; i++){
					if(contents[i].transform.position.x < j)
						direction = 1;
					else
						direction = -1;

					contents [i].transform.localPosition = FindPosition (direction, contents [i].transform.localPosition);
				}
			} else {
				for (int i = 0; i < contents.Count; i++) {
					if (contents[i].transform.position.x > j)
						direction = -2;

					contents [i].transform.localPosition = FindPosition (direction, contents [i].transform.localPosition);
				}
			}
        }

        if (blackMana.Contains(obj)) {
            blackMana.Remove(obj);
        }
    }

	protected virtual Vector3 FindPosition(float scalar, Vector3 pos){
		Vector3 position = pos + scalar * translate;
		return position;
	}
}