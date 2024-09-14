using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerController _playerController;

    PlayerControls _playerControls;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _playerControls = new PlayerControls();
    }

    void OnJump()
    {
        _playerController.Jump();
    }

    void OnMovement(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();

        _playerController.Movement(movementVector);
    }

    void OnRotation(InputValue value)
    {
        Vector2 rotationVector = value.Get<Vector2>();

        _playerController.Rotation(rotationVector);
    }

    void OnDash()
    {
        Debug.Log("Dash");
    }

    void OnCrouch()
    {
        bool crouch = _playerController.isCrouched ? false : true;
        _playerController.isCrouched = crouch;
        Debug.Log("is crouched = " + _playerController.isCrouched);
    }

    void OnSprint()
    {
        _playerController.isSprinting = true;
        Debug.Log("Sprinting On");
    }

    void OnGrabLeft()
    {
        Debug.Log("Grab Left");
    }

    void OnGrabRight()
    {
        Debug.Log("Grab Right");
    }

    void OnTriggerLeft()
    {
        Debug.Log("Trigger Left");
    }

    void OnTriggerRight()
    {
        Debug.Log("Trigger Right");
    }

    void OnMenu()
    {
        Debug.Log("Menu");
    }

    void OnGrabActionLeft()
    {
        Debug.Log("Grab action left");
    }

    void OnGrabActionRight()
    {
        Debug.Log("Grab action right");
    }
}
