using UnityEngine;
using System.Collections;

public class PayButton : MonoBehaviour {

    public ManaPayment manaPayment;

    void OnMouseDown() {
        if(manaPayment.payed)
            manaPayment.ConfirmPayment();
    }
}
