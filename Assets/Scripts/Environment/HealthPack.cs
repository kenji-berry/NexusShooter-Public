using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int itemIndex = 1; // What item is it? 1 = health, 2 = ammo, etc.
    public int healthAmount = 50; // Amount of health to restore

    void Update()
    {
        transform.Rotate(0, 20 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            CollectItem(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void CollectItem(GameObject player)
    {
        Debug.Log("Item collected! Value: " + itemIndex);

        if (itemIndex == 1) // Check if the item is a health pickup
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetComponent<HealthController>().Heal(healthAmount); // Heal the player
            }
        }
    }
}