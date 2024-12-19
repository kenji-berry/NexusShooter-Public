using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public NavMeshAgent agent;
    private EnemyHealthController healthController;
    public SoundController soundController;

    private float detectionRange = 20f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float deaggroRange = 15f;
    public float deaggroTimer = 5f; // Time to maintain aggro after taking damage
    public int damage;
    public float nextAttackTime;
    public float attackCooldown = 2f;
    private float currentDeaggroTime;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private float stopDistance = 1f;
    public bool isDead = false;
    public bool isAttacking = false;

    public float attackRange;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
        bool inAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;

        if (inAttackRange)
        {
            FaceTarget();
            Attack();
        }
        else if (inSightRange)
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

    public void FaceTarget()
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

    public void SetDead()
    {
        animator.SetTrigger("death");
        agent.isStopped = true;
        isDead = true;
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

    public abstract void Attack();
}
