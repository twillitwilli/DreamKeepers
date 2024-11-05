using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using SoT.Interfaces;

public class NPCController : MonoBehaviour, iCooldownable
{
    NavMeshAgent _navAgent;

    [SerializeField]
    LayerMask _ignoreLayers;

    public GameObject nPCModel;

    [SerializeField]
    float _rotationSpeed;

    public TalkToNPC talkToNpc { get; private set; }

    // ------------------ Wandering Variables --------------------------

    [SerializeField]
    bool _canMove;

    [SerializeField]
    Transform
        currentTarget,
        wanderPoint;

    bool _gotToPoint;
    Vector3 _lastPos;

    public float cooldownTimer { get; set; }

    // -------------------------------------------------------------------

    // --------------------- NPC Destinations ----------------------------

    public NPCDestinations[] destinations;
    public NPCDestinations currentDestination { get; private set; }

    bool
        _destinationReached,
        _gettingNewDestination;

    Vector3
        _destinationPos,
        _previousPos;

    // ----------------------------------------------------------------------

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        talkToNpc = GetComponent<TalkToNPC>();

        currentDestination = new NPCDestinations();
    }

    private async void Start()
    {
        DKTime.timeChanged += GetNewDestination;

        // sets npc where they should be on scene load based on time of day
        GetNewDestination();
    }

    private void Update()
    {
        if (CooldownDone() && !_canMove)
        {
            // Cooldown for wander movement
            _canMove = true;
        }

        // if npc isnt moving but should be, create a new waypoint to move to
        if (_canMove && currentDestination != null && !currentDestination.becomeStaticAtDestination && _previousPos == transform.position)
            CheckPathToNewPoint(CreateNewWanderPoint());
    }

    private void LateUpdate()
    {
        if (_canMove)
        {
            if (currentDestination != null && _destinationReached && !currentDestination.becomeStaticAtDestination)
                Wander();

            else
                GoToDestination();
        }

        // if there is a target, the npc will face toward it
        if (currentTarget != null && Vector3.Distance(currentTarget.position, transform.position) > 3)
            FaceTarget(currentTarget);
    }

    // -------------------------------- Wandering Functions ------------------------------------------

    void Wander()
    {
        // if npc got to wander point it will create a new one after cooldown
        if (_gotToPoint && _canMove)
            CheckPathToNewPoint(CreateNewWanderPoint());

        else
        {       
            currentTarget = wanderPoint;

            _navAgent.SetDestination(wanderPoint.position);

            if (Vector3.Distance(transform.position, wanderPoint.transform.position) < 1)
            {
                CooldownDone(true, Random.Range(5, 15));
                _gotToPoint = true;
                _canMove = false;
            }

            _lastPos = transform.position;
            _previousPos = transform.position;
        }
    }

    public void FindNewPathFromDoor()
    {
        CheckPathToNewPoint(CreateNewWanderPoint());
    }

    void CheckPathToNewPoint(Vector3 checkPos)
    {
        RaycastHit hit;
        float range = Vector3.Distance(checkPos, transform.position);

        if (Physics.Raycast(transform.position, checkPos - transform.position, out hit, range, -_ignoreLayers))
        {
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Rock"))
                CreateNewWanderPoint();
        }

        else
            _gotToPoint = false;
    }

    Vector3 CreateNewWanderPoint()
    {
        wanderPoint.SetParent(null);

        float newX = transform.position.x + Random.Range(-12, 12);
        float newZ = transform.position.z + Random.Range(-12, 12);
        wanderPoint.position = new Vector3(newX, transform.position.y, newZ);

        return wanderPoint.position;
    }

    public bool CooldownDone(bool setTimer = false, float cooldownTime = 3)
    {
        if (setTimer)
            cooldownTimer = cooldownTime;

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        else if (cooldownTimer <= 0)
            return true;

        return false;
    }

    // ------------------------------------------------------------------------------------------------

    // -------------------------------- Destination Functions -----------------------------------------

    public void GetNewDestination()
    {
        if (!_gettingNewDestination)
        {
            _gettingNewDestination = true;

            Debug.Log("Getting new destination");

            float currentTime = DKTime.Instance.currentTime;

            Debug.Log("CurrentTime = " + currentTime);

            for (int i = 0; i < destinations.Length; i++)
            {
                // finds which destination the npc should be at based on time of day
                if (destinations[i].startTime < currentTime && destinations[i].endTime > currentTime)
                {
                    Debug.Log("found new destination = " + destinations[i].destinationName);

                    // sets new destination
                    if (currentDestination != null)
                    {
                        currentDestination.destinationName = destinations[i].destinationName;
                        currentDestination.destinationPosition = destinations[i].destinationPosition;
                        currentDestination.becomeStaticAtDestination = destinations[i].becomeStaticAtDestination;
                        currentDestination.canSit = destinations[i].canSit;
                        currentDestination.canSleep = destinations[i].canSleep;
                        currentDestination.canIteractWithPlayer = destinations[i].canIteractWithPlayer;
                        currentDestination.startTime = destinations[i].startTime;
                        currentDestination.endTime = destinations[i].endTime;

                        //SetCurrentDestinationData(destinations[i]);
                        _destinationPos = currentDestination.destinationPosition;
                        _destinationReached = false;
                        _gettingNewDestination = false;

                        return;
                    }

                    else Debug.Log("Current Destination ERROR");
                }
            }
        }
    }

    void GoToDestination()
    {
        _navAgent.SetDestination(_destinationPos);

        // checks to see if destination has been reached
        if (Vector3.Distance(transform.position, _destinationPos) < 1)
            _destinationReached = true;
    }

    // ------------------------------------------------------------------------------------------------

    // ------------------------------------- Facing Direction Functions -------------------------------

    void FaceMovementDirection()
    {
        Vector3 direction = (_previousPos + transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    }

    void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
    }

    bool FacingPlayer(PlayerController player)
    {
        Vector3 currentPos = transform.position;
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 direction = playerPos - currentPos;

        float viewRange = Vector3.Angle(transform.forward, direction);

        if (viewRange < 45)
            return true;

        else return false;
    }
}
