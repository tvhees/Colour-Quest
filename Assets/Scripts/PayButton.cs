using UnityEngine;
using System.Collections;

public class PayButton : MonoBehaviour {

    public ManaPayment manaPayment;

#if UNITY_STANDALONE || UNITY_EDITOR
    void OnMouseDown() {
        ClickAction();
    }
#endif

    void ClickAction() {
        if (manaPayment.payed)
            manaPayment.ConfirmPayment();
    }
}
