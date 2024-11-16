using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private float detectionRange = 10f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float deaggroRange = 15f;
    public float deaggroTimer = 5f; // Time to maintain aggro after taking damage
    private float currentDeaggroTime;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private EnemyHealthController healthController;
    private float stopDistance = 1f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthController = GetComponent<EnemyHealthController>();

        // subscribe to health controller damage event
        healthController.onDamageTaken += OnDamageTaken;

        if (patrolPoints.Length > 0)
        {
            SetNextPatrolPoint();
        }

        agent.stoppingDistance = 0f; // Set initial stopping distance to 0
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Changed aggro check to use detectionRange
        if (distanceToPlayer < detectionRange)
        {
            isAggro = true;
            currentDeaggroTime = Time.time + deaggroTimer;
        }
        else if (distanceToPlayer > deaggroRange && Time.time > currentDeaggroTime)
        {
            ResetAggro();
        }

        if (isAggro)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!isWaiting)
        {
            if (agent.remainingDistance <= stopDistance)
            {
                isWaiting = true;
                waitTimer = patrolWaitTime;
            }
            else
            {
                FaceMovementDirection();
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                SetNextPatrolPoint();
            }
        }
    }

    private void SetNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        FaceTarget();
    }

    private void FaceMovementDirection()
    {
        if (agent.velocity.magnitude > 0.1f) // check if moving
        {
            Vector3 direction = agent.velocity.normalized; // get movement direction
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction); // calculate rotation towards movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // rotate
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized; // get direction towards player
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction); // calculate rotation towards player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // rotate
    }

    public void TakeDamage()
    {
        isAggro = true;
    }

    public void ResetAggro()
    {
        isAggro = false;
    }

    private void OnDestroy()
    {
        // have to cleanup subscription
        if (healthController != null)
        {
            healthController.onDamageTaken -= OnDamageTaken;
        }
    }

    private void OnDamageTaken(int damage)
    {
        isAggro = true;
        currentDeaggroTime = Time.time + deaggroTimer; // Reset deaggro timer when taking damage
    }
}