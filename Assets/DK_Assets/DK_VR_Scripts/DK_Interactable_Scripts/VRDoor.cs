using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class VRDoor : MonoBehaviour
{
    Rigidbody _doorRB;

    [SerializeField]
    bool
        _doorLocked,
        _requiresKey;

    [SerializeField]
    int keyIDRequired;

    private void Awake()
    {
        _doorRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // if the door is set to locked it will make the rigidbody kinematic which will lock the door in place
        if (_doorLocked)
            _doorRB.isKinematic = true;
    }

    public void UnlockDoor()
    {
        // if door requires a key, the key id must match the required key id to unlock 
        if (_requiresKey)
            Debug.Log("A key is required for this door!");

        else
            _doorRB.isKinematic = false;
    }
}
