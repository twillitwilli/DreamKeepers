using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    PlayerController _player;

    public float rotationSpeed;
    public bool lockPosition;
    public Vector3 position;
    public bool lockRotationAxis;
    public bool lockX, lockY, lockZ;
    public Vector3 lockedRotation;

    Transform _target;
    Vector3 _newRotation;

    void Start()
    {
        _player = PlayerController.Instance;
        _target = _player.head;
    }

    void Update()
    {
        _target = _player.head;
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (lockPosition)
            transform.localPosition = position;

        if (lockRotationAxis)
        {
            _newRotation = transform.localEulerAngles;

            if (lockX)
                _newRotation = new Vector3(lockedRotation.x, _newRotation.y, _newRotation.z);

            if (lockY)
                _newRotation = new Vector3(_newRotation.x, lockedRotation.y, _newRotation.z);

            if (lockZ)
                _newRotation = new Vector3(_newRotation.x, _newRotation.y, lockedRotation.z);

            transform.localEulerAngles = _newRotation;
        }
    }
}
