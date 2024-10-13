using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using SoT.Interfaces;

public class NPCController : MonoBehaviour, iCooldownable
{
    PlayerController _playerController;

    NavMeshAgent _navAgent;

    [SerializeField]
    LayerMask _ignoreLayers;

    public GameObject nPCModel;

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

    DKTime _time;

    public NPCDestinations[] destinations;
    public NPCDestinations currentDestination { get; private set; }

    bool _destinationReached;

    Vector3 destinationPos;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _playerController = PlayerController.Instance;
        _time = DKTime.Instance;
    }

    private void Start()
    {
        DKTime.timeChanged += GetNewDestination;

        // sets npc where they should be on scene load based on time of day
        GetNewDestination();
        transform.position = destinationPos;
        _destinationReached = true;
    }

    private void Update()
    {
        if (CooldownDone() && !_canMove)
        {
            // Cooldown for wander movement
            _canMove = true;
        }
    }

    private void LateUpdate()
    {
        if (_canMove)
        {
            if (_destinationReached && !currentDestination.becomeStaticAtDestination)
                Wander();

            else
                GoToDestination();
        }
    }

    // -------------------------------- Wandering Functions ------------------------------------------

    void Wander()
    {
        // if npc got to wander point it will create a new one after cooldown
        if (_gotToPoint && _canMove)
            CheckPathToNewPoint(CreateNewWanderPoint());

        else
        {
            // checks to see if npc reached the wander position and sets cooldown
            if (wanderPoint.position == transform.position)
            {
                _gotToPoint = true;
                _canMove = false;
                CooldownDone(true, Random.Range(5, 15));
            }
                
            currentTarget = wanderPoint;

            float distanceFromPoint = Vector3.Distance(wanderPoint.position, transform.position);
            //WalkingRunningController(distanceFromPoint);

            _navAgent.SetDestination(wanderPoint.position);

            if (distanceFromPoint - 1 <= _navAgent.stoppingDistance)
                _gotToPoint = true;

            _lastPos = transform.position;
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

        float newX = transform.position.x + Random.Range(-4, 4);
        float newZ = transform.position.z + Random.Range(-4, 4);
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

    public async void GetNewDestination()
    {
        // wait 10 - 20 seconds before trying to get a new destination
        await Task.Delay(Random.Range(10000, 20000));

        float currentTime = DKTime.Instance.currentTime;

        for (int i = 0; i < destinations.Length; i++)
        {
            // finds which destination the npc should be at based on time of day
            if (destinations[i].startTime < currentTime && destinations[i].endTime > currentTime)
            {
                //checks to make sure the destination is different from the last one 
                if (currentDestination != null && currentDestination == destinations[i])
                    return;
                
                // sets new destination
                currentDestination = destinations[i];
                destinationPos = destinations[i].destinationPosition;
                _destinationReached = false;
            }
        }
    }

    void GoToDestination()
    {
        _navAgent.SetDestination(destinationPos);
    }
}
