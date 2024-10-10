using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DKEnemyController : MonoBehaviour
{
    PlayerController _playerController;

    DKEnemySpawner _enemySpawner;

    NavMeshAgent _navAgent;

    [SerializeField]
    LayerMask _ignoreLayers;

    public EnemyData enemyStats;

    // ------------------ Wandering Variables --------------------------

    [SerializeField]
    bool _canMove;

    [SerializeField]
    Transform
        currentTarget,
        wanderPoint;

    bool _gotToPoint;
    Vector3 _lastPos;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _playerController = PlayerController.Instance;
        _enemySpawner = DKEnemySpawner.Instance;
    }

    public virtual void LateUpdate()
    {
        if (_canMove)
            Wander();
    }

    public virtual void Wander()
    {
        if (_gotToPoint)
            CheckPathToNewPoint(CreateNewWanderPoint());

        else
        {
            // checks to see if enemy reached the wander position
            if (wanderPoint.position == transform.position)
                _gotToPoint = true;

            currentTarget = wanderPoint;

            float distanceFromPoint = Vector3.Distance(wanderPoint.position, transform.position);
            //WalkingRunningController(distanceFromPoint);

            _navAgent.SetDestination(wanderPoint.position);

            if (distanceFromPoint - 1 <= _navAgent.stoppingDistance)
                _gotToPoint = true;

            _lastPos = transform.position;
        }
    }

    public virtual void CheckPathToNewPoint(Vector3 checkPos)
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

    public virtual Vector3 CreateNewWanderPoint()
    {
        wanderPoint.SetParent(null);

        float newX = transform.position.x + Random.Range(-20, 20);
        float newZ = transform.position.z + Random.Range(-20, 20);
        wanderPoint.position = new Vector3(newX, transform.position.y, newZ);

        return wanderPoint.position;
    }

    private void OnEnable()
    {
        _enemySpawner.currentEnemyCount++;
        wanderPoint.transform.position = transform.position;
    }

    private void OnDisable()
    {
        _enemySpawner.currentEnemyCount--;
    }
}
