using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public NavMeshAgent agent;
    private EnemyHealthController healthController;
    public SoundController soundController;

    public float sightRange = 20f;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    private int currentPatrolIndex = 0;

    [Header("temp")]
    public float deaggroRange = 15f;
    public float deaggroTimer = 5f; // Time to maintain aggro after taking damage
    public int damage;
    private float currentDeaggroTime;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private float stopDistance = 1f;
    public bool isDead = false;
    public bool isAttacking = false;

    [Header("Attack")]
    public float nextAttackTime;
    public float attackCooldown;
    public float attackRange;
    public bool alreadyAttacked;

    [SerializeField] private List<LootItem> possibleLoot; // List of possible items
    public void Start()
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

        playerInSightRange = Vector3.Distance(transform.position, player.transform.position) < sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, player.transform.position) < attackRange;

        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) 
        {
            agent.SetDestination(transform.position);
            FaceTarget();
        }

        animator.SetBool("attacking", playerInAttackRange);
        animator.SetFloat("speed", agent.desiredVelocity.sqrMagnitude);
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

    public virtual void Die()
    {
        float adjustedDropChance = healthController.GetDropChance();
        // First roll - determine if anything drops
        if (UnityEngine.Random.Range(0f, 100f) <= adjustedDropChance)
        {
            // Second roll - determine which item drops
            float totalWeight = possibleLoot.Sum(item => item.dropWeight);
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var lootItem in possibleLoot)
            {
                currentWeight += lootItem.dropWeight;
                if (randomValue <= currentWeight)
                {
                    InstantiateLoot(lootItem.itemPrefab);
                    break;
                }
            }
        }
    
        animator.SetTrigger("death");
        agent.isStopped = true;
        isDead = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }


    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            Vector3 dropPosition = transform.position + Vector3.up * 1f; // Slight offset upward
            GameObject droppedLoot = Instantiate(loot, dropPosition, Quaternion.identity);
        }
    }

    public abstract void Attack();
}
