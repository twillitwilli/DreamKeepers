using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public enum GrabableType
    {
        throwable,
        climbable,
        equipable
    }

    public GrabableType typeOfGrabable;
}
