using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRayCast : MonoBehaviour
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
            Debug.Log("ray hit = " + hit.collider.gameObject);

            Throwable newThrowable;

            if (hit.collider.TryGetComponent<Throwable>(out newThrowable))
            {
                if (_currentGrabableTarget != newThrowable.gameObject)
                    _currentGrabableTarget = newThrowable.gameObject;

                if (!_hitEffect.activeSelf)
                    _hitEffect.SetActive(true);
            }
            else TurnOffHitEffect();
        }
    }

    public void TurnOffHitEffect()
    {
        if (_hitEffect.activeSelf)
            _hitEffect.SetActive(false);
    }
    
    public void ResetTarget()
    {
        _currentGrabableTarget = null;
    }
}
