using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGrabTrigger : MonoBehaviour
{
    public GameObject currentGrabable { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (currentGrabable == null && other.CompareTag("Climbable"))
        {
            Debug.Log("found climbable");

            currentGrabable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            Debug.Log("lost climbable");

            ResetGrabable();
        }
    }

    public void ResetGrabable()
    {
        if (currentGrabable != null)
            currentGrabable = null;
    }
}
