using UnityEngine;
using System.Collections.Generic;

public abstract class Collection<T> : Singleton<T> where T : MonoBehaviour
{
    public float manaScale, gapScale;
    public List<GameObject> contents, blackMana;
    public ManaPool manaPool;

    protected int size;
    private int[] nullValue = new int[3] { 0, 0, 0 };

    public virtual void Reset() {
        while (contents.Count > 0)
        {
            manaPool.SendToPool(contents[0]);
        }
    }

    public void AddMana(GameObject mana) {
        // Add to container
        contents.Add(mana);
        if(mana.GetComponent<Mana>() != null)
            if (mana.GetComponent<Mana>().value.Sum() == 0)
                blackMana.Add(mana);

        // Track mana in container
        size++;

        // Assign local position to the right of existing mana
        mana.transform.parent = transform;
        mana.transform.localScale = manaScale * Vector3.one;
		Vector3 localPoint = new Vector3(0.5f * size * manaScale * gapScale, 0f, 0f);
        mana.transform.localPosition = localPoint;

        // Shift all mana leftward to keep the hand centered
        for (int i = 0; i < contents.Count; i++) {
            contents[i].transform.localPosition = contents[i].transform.localPosition + new Vector3(-0.5f * manaScale * gapScale, 0f, 0f);
        }
    }

    public void Remove(GameObject mana) {
        if (contents.Contains(mana)) {
            int j = contents.IndexOf(mana);
            contents.Remove(mana);
            size--;

            for (int i = j; i < contents.Count; i++)
                contents[i].transform.localPosition = contents[i].transform.localPosition - new Vector3(manaScale * gapScale, 0f, 0f);
        }

        if (blackMana.Contains(mana)) {
            blackMana.Remove(mana);
        }
    }
}