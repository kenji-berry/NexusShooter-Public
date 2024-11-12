using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private EnemyHealthController healthController;

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
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            isAggro = true;
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
            if (agent.remainingDistance <= agent.stoppingDistance)
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
    }
}