using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    BoxCollider _swordTrigger;

    public VRHandController currentHand { get; set; }

    private void Update()
    {
        if (currentHand != null)
        {
            Debug.Log("current hand velocity with sword = " + currentHand.GetHandVelocity());

            if (currentHand.GetHandVelocity() > 500)
                _swordTrigger.isTrigger = true;

            else _swordTrigger.isTrigger = false;
        }
    }
}
