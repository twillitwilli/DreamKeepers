using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNexusController : MonoBehaviour
{
    public UnlockableObject[] unlockablePortals;

    private void Start()
    {
        // Portal Unlock Check
        for (int i = 0; i < DKProgressionChecks.Instance.nexusPortalUnlocks.Length; i++)
        {
            // if portal is unlocked, set the portal to active to make accessable
            if (DKProgressionChecks.Instance.nexusPortalUnlocks[i].completed)
                unlockablePortals[i].unlockableObj.SetActive(true);
        }
    }
}
