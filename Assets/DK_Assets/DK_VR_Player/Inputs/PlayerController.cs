using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputController))]

public sealed class PlayerController : MonoSingleton<PlayerController>
{
    [SerializeField]
    LayerMask _ignoreLayers;

    public GameObject playSpace;

    public Transform head;

    public VRHandController
        leftHand,
        rightHand;

    [SerializeField]
    GroundChecker _groundChecker;

    public PlayerStats playerStats;

    // Player Base Stats
    float
        _walkSpeed = 5,
        _sprintSpeedMultiplier = 2,
        _sneakSpeedReduction = 2,
        _dashDistance = 10,
        _jumpVelocity = 5;

    [HideInInspector]
    public bool
        isGrounded,
        isCrouched,
        isSprinting,
        heightCheck,
        disableMovement,
        sprintEnabled,
        jumpControllerOn,
        climbOn,
        canFly,
        playerCalibrationOn,
        playerHandAdjusterOn,
        playerMoving,
        isGhost;

    public Rigidbody playerRB { get; set; }
    public CapsuleCollider playerCollider { get; set; }

    float collisionRange = 0.75f;

    float playerMovement;

    bool
        floatPlayer,
        canSnapTurn,
        crouchSpeedSet;

    Transform playerOrientation;

    List<Vector3> _playerBodyTracking = new List<Vector3>();

    //public Animator sittingPlayerAnim { get; set; }

    // -------------- PLAYER SETTINGS --------------------

    [HideInInspector]
    public float
        leftJoystickDeadzoneAdjustment = 0.25f,
        rightJoystickDeadzoneAdjustment = 0.5f,
        turnSpeedAdjustment = 1f,
        snapTurnRotationAdjustment = 45;

    //[HideInInspector]
    public bool
        isLeftHanded,
        headOrientation = true,
        snapTurnOn,
        roomScale,
        toggleGrip,
        isStanding = true,
        toggleSprint,
        physicalJumping;

    // ----------------------------------------------------

    // ---------------- Dash Variables -------------------

    bool
        setDashCooldown,
        canDash,
        runDashCooldown;

    float
        cooldownTimer,
        dashCooldownTime = 3;

    Vector2 leftJoystickPos;

    Vector3
        dashPos,
        forwardMovement,
        rightMovement;

    // ------------------------------------------------------

    public override void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        base.Awake();

        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        //sittingPlayerAnim = GetComponent<Animator>();
        playerOrientation = head;
    }

    void Start()
    {
        heightCheck = true;
        OrientationSource();
        canDash = true;
        isGrounded = true;

        if (!isStanding)
        {
            playerCollider.height = 1.87f;
            playSpace.transform.localPosition = new Vector3(0, -0.361f, 0);
            isCrouched = false;
        }
    }

    void FixedUpdate()
    {
        PlayerBodyTracker();
    }

    void Update()
    {
        // Tracks the last 15 positions of the players body while climbing
        if (leftHand.isClimbing || rightHand.isClimbing)
        {
            if (_playerBodyTracking.Count > 60)
                _playerBodyTracking.RemoveAt(0);

            _playerBodyTracking.Add(transform.position);
        }
    }

    void LateUpdate()
    {
        if (!isSprinting && toggleSprint)
            Sprint();

        if (isStanding)
            StandingController();

        if (runDashCooldown)
            canDash = DashCooldown();

        PlayerBounds();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            ChangeMovementSFX();
        }
    }

    // -------------------------------- Movement Controls ----------------------------------

    public void Jump()
    {
        if (!isCrouched && _groundChecker.GroundCheck())
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, _jumpVelocity, playerRB.velocity.z);
            isGrounded = false;
        }
    }

    public void Movement(Vector2 pos)
    {
        //Player not moving
        if (Mathf.Abs(pos.y) < leftJoystickDeadzoneAdjustment && Mathf.Abs(pos.x) < leftJoystickDeadzoneAdjustment)
        {
            //sets player movement to 0
            playerMoving = false;
            playerMovement = _walkSpeed;
            isSprinting = false;

            //stop movement audio

            crouchSpeedSet = false;
        }

        if (!disableMovement)
        {
            if (isCrouched && !crouchSpeedSet) { CrouchSpeedReduction(); }

            //Oreintation (forward and back)
            Vector3 forward = Vector3.Normalize(playerOrientation.transform.forward - new Vector3(0, playerOrientation.transform.forward.y, 0));

            //Player Movement (forward and back)
            if (Mathf.Abs(pos.y) >= leftJoystickDeadzoneAdjustment && CanMoveInDirection(transform.position + forward * playerMovement * pos.y * Time.deltaTime))
            {
                playerMoving = true;
                transform.position += forward * playerMovement * pos.y * Time.deltaTime;
            }

            //Oreintation (side to side)
            Vector3 right = Vector3.Normalize(playerOrientation.transform.right - new Vector3(0, playerOrientation.transform.right.y, 0));

            //Player Movement (side to side)
            if (Mathf.Abs(pos.x) >= leftJoystickDeadzoneAdjustment && CanMoveInDirection(transform.position + right * playerMovement * pos.x * Time.deltaTime))
            {
                playerMoving = true;
                transform.position += right * playerMovement * pos.x * Time.deltaTime;
            }

            leftJoystickPos = pos;
            forwardMovement = forward;
            rightMovement = right;
        }
    }

    bool CanMoveInDirection(Vector3 movePos)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.TransformPoint(playerCollider.center), movePos - transform.position, out hit, collisionRange, -_ignoreLayers))
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall"))
                return false;
        }

        return true;
    }

    public void Rotation(Vector2 pos)
    {
        if (!roomScale)
        {
            //Smooth Turn
            if (!snapTurnOn && Mathf.Abs(pos.x) >= rightJoystickDeadzoneAdjustment)
                transform.RotateAround(head.position, Vector3.up, (pos.x * 90 * turnSpeedAdjustment) * Time.deltaTime);

            //Snap Turn
            else if (snapTurnOn && canSnapTurn)
            {
                float snapValue = 0.0f;

                if (pos.x >= rightJoystickDeadzoneAdjustment) 
                    snapValue = Mathf.Abs(snapTurnRotationAdjustment);

                else if (pos.x <= -rightJoystickDeadzoneAdjustment) 
                    snapValue = -Mathf.Abs(snapTurnRotationAdjustment);

                transform.RotateAround(head.position, Vector3.up, snapValue);
                canSnapTurn = false;
            }

            if (!canSnapTurn && pos.x < rightJoystickDeadzoneAdjustment && pos.x > -rightJoystickDeadzoneAdjustment)
                canSnapTurn = true;
        }
    }

    void Crouch(bool crouched)
    {
        if (crouched)
        {
            if (isSprinting) { playerMovement = _walkSpeed; }
            isSprinting = false;
            CrouchSpeedReduction();

            //stop movement audio

            isCrouched = true;
        }

        else
        {
            playerMovement = _walkSpeed;
            isCrouched = false;
            ChangeMovementSFX();
        }
    }

    void CrouchSpeedReduction()
    {
        playerMovement = playerMovement / _sneakSpeedReduction;
        crouchSpeedSet = true;
    }

    public void Sprint()
    {
        if (!isCrouched && playerMovement == _walkSpeed && !isSprinting)
        {
            isSprinting = true;
            ChangeMovementSFX();
            playerMovement = playerMovement * _sprintSpeedMultiplier;
        }
    }

    // --------------------- Dash Functions ------------------------

    public void Dash()
    {
        if (!isCrouched && playerMoving && canDash)
        {
            //_playerStats.iFrame = true;

            if (Mathf.Abs(leftJoystickPos.y) >= leftJoystickDeadzoneAdjustment)
                dashPos = DashDistanceCheck(transform.position + (forwardMovement * _dashDistance * leftJoystickPos.y));

            else if (Mathf.Abs(leftJoystickPos.x) >= leftJoystickDeadzoneAdjustment)
                dashPos = DashDistanceCheck(transform.position + (rightMovement * _dashDistance * leftJoystickPos.x));

            //_playerComponents.dashEffect.gameObject.SetActive(true);
            //_playerComponents.dashEffect.transform.localPosition = new Vector3(leftJoystickPos.x, 0, leftJoystickPos.y);

            //dash sound effect here

            transform.position = dashPos;
            canDash = false;
            setDashCooldown = true;
            runDashCooldown = true;
        }
    }

    Vector3 DashDistanceCheck(Vector3 dashPosition)
    {
        RaycastHit hit;
        float range = Vector3.Distance(dashPosition, transform.position);

        if (Physics.Raycast(transform.TransformPoint(playerCollider.center), dashPosition - transform.position, out hit, range, -_ignoreLayers))
        {
            if (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall"))
                return hit.point + (transform.position - dashPosition).normalized * collisionRange;
        }

        return dashPosition;
    }

    bool DashCooldown()
    {
        if (setDashCooldown)
        {
            cooldownTimer = dashCooldownTime;
            setDashCooldown = false;
        }

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        else if (cooldownTimer <= 0)
        {
            cooldownTimer = 0;
            //_playerComponents.dashEffect.gameObject.SetActive(false);
            //_playerComponents.visualDashReadyEffect.SetActive(true);
            runDashCooldown = false;
            return true;
        }

        return false;
    }

    // --------------------------------------------------------------

    public void FlightController(bool jumpButtonDown)
    {
        if (jumpButtonDown)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, _jumpVelocity, playerRB.velocity.z);
            floatPlayer = true;
        }

        else
            floatPlayer = false;

        if (floatPlayer)
            playerRB.velocity = playerRB.velocity + new Vector3(0, 10, 0);
    }

    // -------------------------------------------------------------------------------------

    // ----------------------------- Physics & Tracking ------------------------------------

    public void OrientationSource()
    {
        // Headset Orientation
        if (headOrientation)
            playerOrientation = head;

        // Controller Orientation
        else if (!headOrientation)
            playerOrientation = leftHand.transform;
    }

    // Keeps player body collider under the head position
    void PlayerBodyTracker()
    {
        Vector3 colliderCenter = Vector3.zero;

        if (isStanding)
        {
            float headHeight = Mathf.Clamp(head.localPosition.y, 1 / 2, 2);
            playerCollider.height = headHeight;
            colliderCenter.y = playerCollider.height / 2;
        }

        colliderCenter.x = head.localPosition.x;
        colliderCenter.z = head.localPosition.z;
        playerCollider.center = colliderCenter;
    }

    void StandingController()
    {
        if (heightCheck)
        {
            playSpace.transform.localPosition = new Vector3(0, 0, 0);
            //sittingPlayerAnim.SetBool("isCrouched", false);
            //sittingPlayerAnim.enabled = false;
            heightCheck = false;
        }

        if (isCrouched && playerCollider.height > 1.2)
            Crouch(false);

        else if (!isCrouched && playerCollider.height < 1.2)
            Crouch(true);

        if (physicalJumping && playerCollider.height > 2)
            Jump();
    }

    public void SittingController()
    {
        if (heightCheck)
        {
            //sittingPlayerAnim.enabled = true;
            heightCheck = false;
        }

        if (!isCrouched)
        {
            //sittingPlayerAnim.SetBool("isCrouched", true);
            Crouch(true);
        }

        else if (isCrouched)
        {
            //sittingPlayerAnim.SetBool("isCrouched", false);
            Crouch(false);
        }
    }

    public void ThrowPlayerBody()
    {
        Debug.Log("Throw Player from climb");

        // Get the direction the player body will be thrown
        Vector3 direction = _playerBodyTracking[_playerBodyTracking.Count - 1] - _playerBodyTracking[0];

        // Add force in the direction the player will be thrown
        playerRB.AddForce(direction * 100000);

        // Clear the player body tracking list 
        _playerBodyTracking.Clear();
    }

    // --------------------------------------------------------------------------------------

    private void ChangeMovementSFX()
    {
        if (isGrounded)
        {
            if (!isSprinting && !isCrouched) { } //walking sfx

            else if (isSprinting) { } //runnning sfx
        }
    }

    public void DefaultPlayerSettings()
    {
        Debug.Log("default player setttings ran");

        DefaultAttachmentSettings();
    }

    public void DefaultAttachmentSettings()
    {
        //_playerComponents.belt.backAttachments = 0;
        //_playerComponents.belt.heightStandingPlayer = 0.65f;
        //_playerComponents.belt.heightSittingPlayer = 0.185f;
        //_playerComponents.belt.zAdjustmentForSittingPlayer = 0.145f;
    }

    void PlayerBounds()
    {
        // If player goes outside of these bounds they will be moved back to the current scene spawn point
        if (transform.position.y < -1500 || transform.position.y > 1500)
            transform.position = SceneSpawnLocations.Instance.spawnLocations[DKGameManager.Instance.spawnLocation].spawnLocation.position;
    }
}