using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class HandRayCast : MonoBehaviour
{
    [SerializeField]
    VRHandController _hand;

    [SerializeField]
    LayerMask _ignoreLayers;

    [SerializeField]
    GameObject _hitEffect;

    public GameObject _currentGrabableTarget { get; private set; }

    float range = 10;

    private void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * range;
        Debug.DrawRay(transform.position, forward, Color.green);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward) * range;

        if (_hand.currentGrabable == null && Physics.Raycast(transform.position, forward, out hit, range, -_ignoreLayers))
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
