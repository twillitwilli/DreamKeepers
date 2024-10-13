using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PhysicalGrabTrigger : MonoBehaviour
{
    public Grabable currentGrabable { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        Grabable newGrabable;

        if (currentGrabable == null && other.gameObject.TryGetComponent<Grabable>(out newGrabable))
            currentGrabable = newGrabable;
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
