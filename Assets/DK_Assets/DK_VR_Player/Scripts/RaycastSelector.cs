using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelector : MonoBehaviour
{
    [SerializeField]
    LayerMask _ignoreLayers;

    public void ShootRaycastSelector()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;

        if (Physics.Raycast(transform.position, forward, out hit, 5, -_ignoreLayers))
        {
            Interactable newInteractable;

            if (hit.collider.gameObject.TryGetComponent<Interactable>(out newInteractable))
                newInteractable.Interact();
        }
    }
}
