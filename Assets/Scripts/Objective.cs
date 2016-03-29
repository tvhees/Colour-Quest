using UnityEngine;
using System.Collections;

public abstract class Objective : ClickableObject {

    public int[] cost;

    protected abstract void Bonus();

	public override void ClickAction ()
	{
	}
}
