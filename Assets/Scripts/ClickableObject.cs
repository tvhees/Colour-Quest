using UnityEngine;
using System.Collections;

public abstract class ClickableObject : MonoBehaviour {

    public abstract void ClickAction();

    public virtual void KillTile(bool dead) {

    }
}
