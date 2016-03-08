using UnityEngine;
using System.Collections.Generic;

public abstract class Collection<T> : Singleton<T> where T : MonoBehaviour
{
    public float manaScale, gapScale;
    public List<GameObject> contents, black;
    protected int size;

    public void AddMana(GameObject mana) {
        // Add to container
        contents.Add(mana);

        // Assign local position to the right of existing mana
        mana.transform.parent = transform;
        mana.transform.localScale = manaScale * Vector3.one;
		Vector3 localPoint = new Vector3((size + 0.6f) * manaScale * gapScale, 0f, 0f);
        mana.transform.localPosition = localPoint;

        // Track mana in container
        size++;
    }

    public void RemoveMana(GameObject mana) {
        if (contents.Contains(mana)) {
            int j = contents.IndexOf(mana);
            contents.Remove(mana);
            size--;

            for (int i = j; i < contents.Count; i++)
                contents[i].transform.localPosition = contents[i].transform.localPosition - new Vector3(manaScale * gapScale, 0f, 0f);
        }

        if (black.Contains(mana)) {
            black.Remove(mana);
        }
    }
}