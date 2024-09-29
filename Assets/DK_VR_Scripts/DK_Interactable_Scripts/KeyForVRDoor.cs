using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyForVRDoor : MonoBehaviour
{
    [SerializeField]
    int keyID;

    private void OnTriggerEnter(Collider other)
    {
        VRDoor doorLock;

        if (other.TryGetComponent<VRDoor>(out doorLock))
            doorLock.UnlockDoor(keyID);
    }
}
