using UnityEngine;

public class ArmourPack : MonoBehaviour
{
    public HealthController.ArmourTier armourTier; // The armour tier this pack provides

    void Update()
    {
        transform.Rotate(40 * Time.deltaTime, 0, 0);
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
        HealthController healthController = player.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.SetArmourTier(armourTier); // Set the player's armour tier
        }
    }
}