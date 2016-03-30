using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Collection<T> : Singleton<T> where T : MonoBehaviour
{
    public float objScale, gapScale;
    public List<GameObject> contents, blackMana;
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
		centered = true;

		while (contents.Count > 0)
		{
			pool.SendToPool(contents[0]);
		}

		manaList = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

		translate = new Vector3 (0.5f * objScale * gapScale, 0f, 0f);
	}

    public virtual IEnumerator AddObj(GameObject obj) {
        contents.Add(obj);

		// If we're adding a mana object, we need to check if it's black mana
		// and also add its value to the container's contents summary if required
		if (obj.GetComponent<Mana> () != null) {
			if (obj.GetComponent<Mana>().value.Sum () == 0)
				blackMana.Add (obj);

			if(valueOnAdd)
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
			yield return StartCoroutine (MoveObject (obj, transform.TransformPoint(localPoint)));

            Debug.Log(size + " " + this.name);

			// Shift all mana to keep the hand centered
			for (int i = 0; i < size; i++) {
				localPoint = FindPosition (Mathf.Pow(-1,size), contents [i].transform.localPosition);
                StartCoroutine(MoveObject (contents [i], transform.TransformPoint(localPoint)));
			}
		} else {
			localPoint = FindPosition (size, new Vector3(0f, 0f, 0f));

			yield return StartCoroutine (MoveObject (obj, transform.TransformPoint(localPoint)));
		}
	}

	protected virtual IEnumerator MoveObject(GameObject obj, Vector3 localPoint){
		yield return StartCoroutine (obj.GetComponent<ClickableObject> ().SmoothMovement (localPoint));
	} 

	protected virtual void ChangeValue(GameObject obj, bool increase){
		int[] value = obj.GetComponent<Mana> ().value;
		if (increase) {
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

			// Shift objects towards position of the removed object
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