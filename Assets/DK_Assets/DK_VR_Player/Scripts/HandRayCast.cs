using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class HandRayCast : MonoBehaviour
{
    [SerializeField]
    VRHandController _hand;

    [SerializeField]
    LayerMask _ignoreLayers;

    [SerializeField]
    GameObject _hitEffect;

    public GameObject _currentGrabableTarget { get; private set; }

    float
        _telekineticRange = 10,
        _interactionRange = 5;

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * _telekineticRange;
        Debug.DrawRay(transform.position, forward, Color.green);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * _telekineticRange;

        // Telekinetic Grab
        if (_hand.currentGrabable == null && Physics.Raycast(transform.position, forward, out hit, _telekineticRange, -_ignoreLayers))
        {
            Throwable newThrowable;

            if (hit.collider.TryGetComponent<Throwable>(out newThrowable))
            {
                if (_currentGrabableTarget != newThrowable.gameObject)
                    _currentGrabableTarget = newThrowable.gameObject;

                if (!_hitEffect.activeSelf)
                {
                    _hitEffect.SetActive(true);
                    _hitEffect.transform.position = hit.transform.position;
                }
                    
            }
            else TurnOffHitEffect();
        }

        // Interaction Raycast
        else if (Physics.Raycast(transform.position, forward, out hit, _interactionRange, -_ignoreLayers))
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

    public void TurnOffHitEffect()
    {
        if (_hitEffect.activeSelf)
            _hitEffect.SetActive(false);
        _hitEffect.transform.localPosition = new Vector3(0, 0, 0);

        ResetTarget();
    }
    
    public void ResetTarget()
    {
        _currentGrabableTarget = null;
    }
}
