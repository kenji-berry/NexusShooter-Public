using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRadius = 5f; // Radius of the explosion
    public float explosionForce = 700f; // Force applied to nearby objects
    public int damage = 50; // Damage dealt to enemies

    public float explosionDelay = 40f; // Delay before the grenade explodes
    public GameObject explosionEffect; // Optional particle effect for explosion


   void Start()
    {
        // Start the countdown timer
        Invoke(nameof(Explode), explosionDelay);
    }

//    private void OnCollisionEnter(Collision collision)
//     {
//         // Explode on collision
//         Explode();
//     }

    private void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        }

        // Detect nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Apply damage to enemies
            EnemyHealthController enemy = nearbyObject.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Destroy the grenade object
        Destroy(gameObject, 2f);
    }
}
