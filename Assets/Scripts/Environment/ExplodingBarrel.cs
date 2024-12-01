using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour
{
    private int barrelHealth = 1;
    public GameObject explosionEffect;
    public float explosionRadius = 5f;
    public float explosionForce = 700f;
    public int maxDamage = 100;

    private bool hasExploded = false;

    public void TakeDamage(int damage)
    {
        if (hasExploded) return;
        
        barrelHealth -= damage;
        if (barrelHealth <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Get all colliders within explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hitColliders)
        {
            // Calculate damage based on distance
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            float damageMultiplier = 1f - (distance / explosionRadius);
            int calculatedDamage = Mathf.RoundToInt(maxDamage * damageMultiplier);

            // Apply damage to player
            HealthController playerHealth = hit.GetComponent<HealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(calculatedDamage);

                // Apply knockback to player
                PlayerController playerController = hit.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    Debug.Log(transform.position);
                    Vector3 knockbackDirection = (hit.transform.position - transform.position).normalized;
                    Debug.Log(knockbackDirection);
                    playerController.ApplyKnockback(knockbackDirection, explosionForce, 0.5f); // Apply knockback over 0.5 seconds
                }
            }

            // Apply damage to enemies
            EnemyHealthController enemyHealth = hit.GetComponent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(calculatedDamage);
            }
        }

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
