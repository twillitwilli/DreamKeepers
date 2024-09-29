using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField]
    bool _sleepableBed;

    public void Sleep()
    {
        if (_sleepableBed)
            Debug.Log("Go to sleep");
    }
}
