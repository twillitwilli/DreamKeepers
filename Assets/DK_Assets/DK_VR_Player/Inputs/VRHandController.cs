using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class VRHandController : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    VRHandController _oppositeHand;

    [SerializeField]
    PhysicalGrabTrigger _physicalGrabTrigger;

    [SerializeField]
    HandAnimatorController _handAnimator;

    // ---------------- Throwable Variables -----------------

    [SerializeField]
    HandRayCast _handRayCast;

    public Transform _throwableHeld;

    List<Vector3> _handTrackingPos = new List<Vector3>();

    public float throwVelocity = 1000f;

    public GameObject currentGrabable { get; private set; }
    Rigidbody _currentGrabableRB;
    Throwable _currentThrowable;


    // ---------------------------------------------------------

    // ------------------ Climbing Variables -------------------

    GameObject _currentClimbableObject;

    Vector3
        _handStartPos,
        _climbablePrevPos;

    Transform _climbableObject;

    public bool isClimbing { get; private set; }

    void Update()
    {
        // Tracks last 15 positions of the hand, if is holding something
        if (currentGrabable != null)
        {
            if (_handTrackingPos.Count > 15)
                _handTrackingPos.RemoveAt(0);

            _handTrackingPos.Add(transform.position);
        }
    }

    void LateUpdate()
    {
        if (isClimbing)
            Climbing();
    }

    // ------------------------------------ Grab Functions -------------------------------------

    public void GrabObject(bool grab)
    {
        // If there is no physical object to grab
        if (_physicalGrabTrigger.currentGrabable == null)
        {
            // Grab Object
            if (grab && currentGrabable == null && _handRayCast._currentGrabableTarget != null)
            {
                GrabThrowableObject();
                _handAnimator.ChangeAnimation("TelekineticHold");
            }

            // Drop Object
            else if (!grab && currentGrabable != null)
            {
                ThrowObject();
                _handAnimator.ChangeAnimation("Idle");
            }
        }
        
        // If there is a physical object to grab
        else
        {
            if (grab)
            {
                Climb();
                _handAnimator.ChangeAnimation("TelekineticHold");
            }

            else
            {
                ReleaseClimb();
                _handAnimator.ChangeAnimation("Idle");
            }
        }
    }

    // ------------------------------------------------------------------------------------------

    // ---------------------------------- Throwable Functions -----------------------------------

    void GrabThrowableObject()
    {
        // Turn off visual grab effect & reset raycast target
        currentGrabable = _handRayCast._currentGrabableTarget;
        _currentThrowable = currentGrabable.GetComponent<Throwable>();
        _handRayCast.TurnOffHitEffect();
        _handRayCast.ResetTarget();

        //Check to see if opposite hand is holding object
        if (currentGrabable.transform.parent == _oppositeHand._throwableHeld)
            _oppositeHand.GrabObject(false);

        // Get RB and adjust settings
        _currentGrabableRB = currentGrabable.GetComponent<Rigidbody>();
        _currentGrabableRB.useGravity = false;
        _currentGrabableRB.isKinematic = true;

        // Set grabable trigger to true to avoid player collisions
        currentGrabable.GetComponent<Collider>().isTrigger = true;

        // Parent object to the throwable transform and reset transform
        currentGrabable.transform.SetParent(_throwableHeld);
        currentGrabable.transform.localPosition = new Vector3(0, 0, 0);
        currentGrabable.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    void ThrowObject()
    {
        // Unparent object from hand
        currentGrabable.transform.SetParent(null);
        _currentGrabableRB.isKinematic = false;

        // Get direction & add force to the object being thrown
        Vector3 direction = _handTrackingPos[_handTrackingPos.Count - 1] - _handTrackingPos[0];
        _currentGrabableRB.AddForce(direction * throwVelocity);

        //Get throwable velocity
        _currentThrowable.throwableVelocity = Vector3.Magnitude(direction * throwVelocity);
        Debug.Log("Throwable velocity = " + _currentThrowable.throwableVelocity);

        // Reset RB settings
        _currentGrabableRB.useGravity = true;

        // Turn collider trigger off
        currentGrabable.GetComponent<Collider>().isTrigger = false;

        // Reset grabable data on hand
        currentGrabable = null;
        _currentGrabableRB = null;
        _currentThrowable = null;
    }

    // -------------------------------------------------------------------------------------------

    // ----------------------------------- Climbing Functions ------------------------------------
    public void TogglePhysicalGrabTrigger(bool on)
    {
        _physicalGrabTrigger.gameObject.SetActive(on);

        if (!_physicalGrabTrigger.gameObject.activeSelf)
            _physicalGrabTrigger.ResetGrabable();
    }

    void Climb()
    {
        isClimbing = true;

        // Release opposite hand from climbing, only if it is climbing already
        if (_oppositeHand.isClimbing)
            _oppositeHand.ReleaseClimb();

        // Starting Climb Settings
        _currentClimbableObject = _physicalGrabTrigger.currentGrabable;
        _handStartPos = transform.position;
        _climbableObject = _currentClimbableObject.transform;
        _climbablePrevPos = _currentClimbableObject.transform.position;

        Debug.Log("Player is Climbing with " + gameObject);
    }

    public void ReleaseClimb()
    {
        isClimbing = false;
        _currentClimbableObject = null;

        // player is no longer climbing
        if (!_oppositeHand.isClimbing)
        {
            // Enable player settings
            _playerController.disableMovement = false;
            _playerController.playerCollider.enabled = true;
            _playerController.playerRB.useGravity = true;

            _playerController.ThrowPlayerBody();
        }

        Debug.Log("Player released climbing grip with " + gameObject);
    }

    void Climbing()
    {
        // Reset player velocity
        _playerController.playerRB.velocity = new Vector3(0, 0, 0);

        // Disable player settings
        _playerController.disableMovement = true;
        _playerController.playerCollider.enabled = false;
        _playerController.playerRB.useGravity = false;

        // -------------- Climbing Movement ---------------------

        // Check to see if the climbable object has moved
        Vector3 climbableObjectPos = _climbableObject.position - _climbablePrevPos;

        // Adjust the hand position according to the movement of the climbable object
        _handStartPos += climbableObjectPos;

        // Check to see how much the hand has moved against the adjusted position
        Vector3 handMovement = transform.position - _handStartPos;

        // Combine the 2 movement vectors for the total movement to be applied to player body
        Vector3 playerBodyMovement = climbableObjectPos - handMovement;

        // Move the player
        _playerController.transform.position += playerBodyMovement;

        // Last climbing position
        _climbablePrevPos = _climbableObject.position;
    }

    // --------------------------------------------------------------------------------------------
}
