using System.Collections;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    public int damage = 10000; // Damage to apply to the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyDamage(other);
            Debug.Log("Player entered lava");
        }
    }

    private void ApplyDamage(Collider player)
    {
        // Apply damage to the player
        HealthController playerHealth = player.GetComponent<HealthController>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}