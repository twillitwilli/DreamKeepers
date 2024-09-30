using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PhysicalGrabTrigger : MonoBehaviour
{
    public GameObject currentGrabable { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (currentGrabable == null && other.CompareTag("Climbable"))
            currentGrabable = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
            ResetGrabable();
    }

    public void ResetGrabable()
    {
        if (currentGrabable != null)
            currentGrabable = null;
    }
}
