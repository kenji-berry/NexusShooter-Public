using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healthAmount = 50;
    public float bounceHeight = 0.1f;
    public float bounceSpeed = 3f;
    private Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
    }
    void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0);
        float newY = originalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight; // Move the item up and down
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z); // Set the new position
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
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetComponent<HealthController>().Heal(healthAmount); // Heal the player
            }
        
    }
}