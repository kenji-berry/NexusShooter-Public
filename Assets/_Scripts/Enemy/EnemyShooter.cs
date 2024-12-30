using UnityEngine;
using UnityEngine.AI;

public class EnemyShooter : Enemy
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 200f;

    void Awake()
    {
        damage = 15;
        attackRange = 10f;
        attackCooldown = 2f;
    }

    public override void Attack()
    {
        if (bulletPrefab == null) return;

        FaceTarget();
        
        if (Time.time >= nextAttackTime)
        {
            float distanceToPlayer = (player.transform.position - firePoint.position).magnitude;
            float timeToPlayer = Mathf.Min(distanceToPlayer / bulletSpeed, 0.2f);

            Vector3 predictedPosition = player.transform.position + player.GetComponent<PlayerController>().characterVelocity * timeToPlayer;
            Vector3 directionToPlayer = (predictedPosition - firePoint.position).normalized;

            // Spawn bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.velocity = directionToPlayer * bulletSpeed;

            Destroy(bullet, 5f);

            animator.SetBool("attacking", false);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}