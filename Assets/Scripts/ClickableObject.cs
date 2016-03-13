using UnityEngine;
using System.Collections;

public abstract class ClickableObject : MonoBehaviour {

#if UNITY_STANDALONE || UNITY_EDITOR
    public void OnMouseDown() {
        ClickAction();
    }
#endif

    public abstract void ClickAction();

    public virtual void KillTile(bool dead) {

    }
}
