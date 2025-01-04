using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    // Optional: Add any additional settings, like damage over time, effects, etc.

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered lava");
            // Attempt to get the PlayerController component from the colliding object
            HealthController healthController = other.GetComponent<HealthController>();

            if (healthController != null)
            {
                // Call the SetDead method to handle player death
                healthController.InstaKill();
            }
            else
            {
                Debug.LogWarning("healthController component not found on the player object.");
            }
        }
    }
}
