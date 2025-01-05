using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayTeleporter : MonoBehaviour
{
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private string playerTag = "Player";   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (teleportDestination == null)
            {
                Debug.LogError("Teleport destination is not assigned!");
                return;
            }

            // Disable the collider to avoid any issues during teleportation
            other.enabled = false;

            // Teleport the object to the destination
            other.transform.position = teleportDestination.position;

            // Re-enable the collider after teleportation
            other.enabled = true;

            Debug.Log("teleported");
        }
    }
}
