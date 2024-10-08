using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNexusUnlock : MonoBehaviour
{
    [SerializeField]
    int portalIdx;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player;

        // Checks to see if player entered trigger
        if (other.gameObject.TryGetComponent<PlayerController>(out player) && !DKProgressionChecks.Instance.nexusPortalUnlocks[portalIdx].completed)
        {
            DKProgressionChecks.Instance.nexusPortalUnlocks[portalIdx].completed = true;
        }
    }
}
