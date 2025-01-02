using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healthAmount = 50;

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
            playerController.GetComponent<HealthController>().Heal(healthAmount);
        }
    }
}