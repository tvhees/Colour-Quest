using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour {

    public int[] cost;

    protected abstract void Bonus();
}
