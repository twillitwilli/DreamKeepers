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

    CapsuleCollider _trigger;

    List<Throwable> _throwableObjects;
    List<Interactable> _interactableObjects;

    float
        _telekineticRange = 10,
        _interactionRange = 5;

    private void Start()
    {
        _trigger = GetComponent<CapsuleCollider>();
    }

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
    }

    public void TurnTriggerOn()
    {
        _trigger.enabled = true;
    }

    public void TurnTriggerOff()
    {
        _trigger.enabled = false;

        _throwableObjects.Clear();
        _interactableObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Throwable newThrowable;
        Interactable newInteractable;

        // checks if object is a throwable object
        // then checks to see if there are any obstructions between the throwable object and the hand
        // then checks to make sure the object is within range
        if (other.gameObject.TryGetComponent<Throwable>(out newThrowable) && !CheckForObstructions(newThrowable.gameObject) && _telekineticRange > Vector3.Distance(_hand.transform.position, newThrowable.transform.position))
            _throwableObjects.Add(newThrowable);

        // checks if object is interactable
        // checks to make sure there are no obstructions between the interactable and the hand
        // checks to make sure the interactable is within range
        else if (other.gameObject.TryGetComponent<Interactable>(out newInteractable) && !CheckForObstructions(newInteractable.gameObject) && _interactionRange > Vector3.Distance(_hand.transform.position, newInteractable.transform.position))
            _interactableObjects.Add(newInteractable);
    }

    private void OnTriggerExit(Collider other)
    {
        Throwable newThrowable;
        Interactable newInteractable;

        // if the object is a throwable object and exists in the throwable list, it will remove it
        if (other.gameObject.TryGetComponent<Throwable>(out newThrowable) && _throwableObjects.Contains(newThrowable))
            _throwableObjects.Remove(newThrowable);

        // if the object is a interactable object and exists in the interactable list, it will remove it
        else if (other.gameObject.TryGetComponent<Interactable>(out newInteractable) && _interactableObjects.Contains(newInteractable))
            _interactableObjects.Remove(newInteractable);
    }

    bool CheckForObstructions(GameObject objectToCheck) //not in use
    {
        RaycastHit hit;

        //"vector3.distance" returns a float value distance between 2 vector3 points ("to" point 1, "from" point 2)
        float range = Vector3.Distance(objectToCheck.transform.position, _hand.transform.position);

        //Raycast (origin of where the ray shoots from, direction the ray shoots (always "to - from" for the direction to be for specific point between to objects, "out hit" automatically sends it to the raycasthit hit variable, "range" is just the range of the ray, )
        if (Physics.Raycast(_hand.transform.position, objectToCheck.transform.position - _hand.transform.position, out hit, range, -_ignoreLayers))
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ground"))
            {
                //Debug.Log("ray hit wall or ground");
                return true;
            }
        }

        // no obstructions found
        return false;
    }

    GameObject GetGrabable()
    {
        // checks to see if there are any throwable objects
        if (_throwableObjects.Count > 0)
        {
            GameObject closestGrabable = null;

            for (int i = 0; i < _throwableObjects.Count; i++)
            {
                // if there isnt a grabable assigned, assign throwabale object
                if (closestGrabable == null)
                    closestGrabable = _throwableObjects[i].gameObject;

                // if new throwable being checked is closer to the hand than the current on, reasign new throwable the closest grabable
                else if (Vector3.Distance(closestGrabable.transform.position, _hand.transform.position) > Vector3.Distance(_throwableObjects[i].transform.position, _hand.transform.position))
                    closestGrabable = _throwableObjects[i].gameObject;
            }

            // returns closest grabable found
            return closestGrabable;
        }

        // there is no throwable objects
        return null;
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
