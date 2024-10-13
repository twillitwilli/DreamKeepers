using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCDestinations
{
    public string destinationName;

    public bool
        becomeStaticAtDestination,
        canSit,
        canSleep,
        canEnterDoor,
        canIteractWithPlayer;

    public Vector3 destinationPosition;

    public float
        startTime,
        endTime;
}
