using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public GameObject player;
    public Animator animator;

    private float detectionRange = 10f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float deaggroRange = 15f;
    public float deaggroTimer = 5f; // Time to maintain aggro after taking damage
    public float meleeRange = 2f;
    public int damage = 20;
    private float nextAttackTime = 0f;
    public float attackCooldown = 2f;
    private float currentDeaggroTime;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private EnemyHealthController healthController;
    private float stopDistance = 1f;
    public bool isDead = false;
    public bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (isDead) { return; }

        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        bool inSightRange = Vector3.Distance(transform.position, player.transform.position) < detectionRange;
        bool inAttackRange = Vector3.Distance(transform.position, player.transform.position) < meleeRange;

        if (inAttackRange)
        {
            Attack();
        } else if (inSightRange)
        {
            ChasePlayer();
        } else
        {
            Patrol();
        }
    }

    void Attack()
    {
        agent.SetDestination(transform.position);

        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("punch");

            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetComponent<HealthController>().TakeDamage(damage);
            }

            nextAttackTime = Time.time + attackCooldown; // Set next attack time
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
        agent.SetDestination(player.transform.position);
        animator.SetBool("isRunning", true);
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
        Vector3 direction = (player.transform.position - transform.position).normalized; // get direction towards player
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
        animator.SetBool("isRunning", false);
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