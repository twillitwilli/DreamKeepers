using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandController : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    HandRayCast _handRayCast;

    [SerializeField]
    Transform _throwableHeld;

    public GameObject currentGrabable { get; private set; }
    Rigidbody currentGrabableRB;

    public void GrabObject(bool grab)
    {
        // Grab Object
        if (grab && currentGrabable == null && _handRayCast._currentGrabableTarget != null)
        {
            Debug.Log("grabbing object");

            currentGrabable = _handRayCast._currentGrabableTarget;
            _handRayCast.TurnOffHitEffect();
            _handRayCast.ResetTarget();

            currentGrabableRB = currentGrabable.GetComponent<Rigidbody>();
            currentGrabableRB.useGravity = false;
            currentGrabableRB.isKinematic = true;

            currentGrabable.transform.SetParent(_throwableHeld);
            currentGrabable.transform.localPosition = new Vector3(0, 0, 0);
            currentGrabable.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        // Drop Object
        else if (!grab && currentGrabable != null)
        {
            Debug.Log("drop object");
            currentGrabable.transform.SetParent(null);
            currentGrabableRB.useGravity = true;
            currentGrabableRB.isKinematic = false;
            currentGrabable = null;
            currentGrabableRB = null;
        }
    }
}
