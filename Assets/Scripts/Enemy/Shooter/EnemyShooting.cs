using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Animator animator;
    public EnemyAI enemyAI;
    public GameObject player;  // Location of player using transform properties
    public float shootingRange = 10f;  // Range within which the enemy can shoot
    public float fireRate = 1f; // Time between shots
    public GameObject bulletPrefab; // Bullet prefab to shoot
    public Transform firePoint; // Position where the bullet will be spawned
    public float bulletSpeed = 20f; // Speed of the bullet
    private float nextFireTime = 0f;  // Time to fire next bullet

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (GetComponent<EnemyAI>().isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Calculate the distance between the enemy and the player

        if (distanceToPlayer < shootingRange && Time.time >= nextFireTime) // Check if the player is within shooting range and it's time to fire
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(transform.position);
            enemyAI.isAttacking = true;

            // shoot function is called during the shoot animation
            animator.SetTrigger("shoot");
            //Shoot();

            enemyAI.isAttacking = false;
            nextFireTime = Time.time + 1f / fireRate; // Set next time to fire
        }

        FaceTarget(); // Always face the player when in range
    }
    void Shoot()
    {
        if (bulletPrefab == null) return;

        
        // Vector3 directionToPlayer = (player.position - firePoint.position).normalized;// Calculate direction to player's center (assuming player pivot is at center)
        
        float distanceToPlayer = (player.transform.position - firePoint.position).magnitude;
        float timeToPlayer = Mathf.Min(distanceToPlayer / bulletSpeed, 0.2f);

        Vector3 predictedPosition = player.transform.position + player.GetComponent<PlayerController>().characterVelocity * timeToPlayer;
        Vector3 directionToPlayer = (predictedPosition - firePoint.position).normalized;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        
        rb.velocity = directionToPlayer * bulletSpeed;
        
        Destroy(bullet, 5f);
    }
    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keep enemy rotation only on Y axis
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

