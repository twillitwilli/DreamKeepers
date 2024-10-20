using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastSelector : MonoBehaviour
{
    [SerializeField]
    LayerMask _ignoreLayers;

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;

        if (Physics.Raycast(transform.position, forward, out hit, 5, -_ignoreLayers))
        {
            Button interactionButton;

            if (hit.collider.gameObject.TryGetComponent<Button>(out interactionButton))
            {
                // Open Chest
                Chest newChest;
                if (interactionButton.gameObject.TryGetComponent<Chest>(out newChest))
                {
                    newChest.OpenChest();
                }
            }
        }
    }
}
