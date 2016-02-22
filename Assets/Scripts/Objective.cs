using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour {

    public int[] objectiveCost;

    protected abstract void Bonus();
}
