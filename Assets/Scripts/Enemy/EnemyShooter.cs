using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : MonoBehaviour
{
    public GameObject player;
    public Animator animator;
    public Transform firePoint;

    private float detectionRange = 10f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;
    public float deaggroRange = 15f;
    public float deaggroTimer = 5f; // Time to maintain aggro after taking damage
    private float currentDeaggroTime;
    private float shootCooldown = 2f;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isAggro = false;
    private EnemyHealthController healthController;
    private float stopDistance = 1f;
    public bool isDead = false;
    public bool isAttacking = false;
    public float shootingRange = 10f;  // Range within which the enemy can shoot
    public float fireRate = 1f; // Time between shots
    public GameObject bulletPrefab; // Bullet prefab to shoot
    public float bulletSpeed = 20f; // Speed of the bullet
    private float nextFireTime = 0f;  // Time to fire next bullet

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
        bool inAttackRange = Vector3.Distance(transform.position, player.transform.position) < shootingRange;

        if (inAttackRange)
        {
            FaceTarget();
            Shoot();
        }
        else if (inSightRange)
        {
            FaceTarget();
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

    void Shoot()
    {
        if (bulletPrefab == null) return;

        if (Time.time >= nextFireTime)
        {
            Debug.Log("SHOOTING");
            animator.SetTrigger("shoot");
            animator.SetBool("isRunning", false);

            float distanceToPlayer = (player.transform.position - firePoint.position).magnitude;
            float timeToPlayer = Mathf.Min(distanceToPlayer / bulletSpeed, 0.2f);

            Vector3 predictedPosition = player.transform.position + player.GetComponent<PlayerController>().characterVelocity * timeToPlayer;
            Vector3 directionToPlayer = (predictedPosition - firePoint.position).normalized;

            // Spawn bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.velocity = directionToPlayer * bulletSpeed;

            Destroy(bullet, 5f);

            nextFireTime = Time.time + shootCooldown;
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