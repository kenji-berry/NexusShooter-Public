using UnityEngine;

public class ArmourPack : MonoBehaviour
{
    public HealthController.ArmourTier armourTier; // The armour tier this pack provides

    private Vector3 originalPosition;
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.5f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // Changed rotation to X-axis (first parameter)
        transform.Rotate(40 * Time.deltaTime, 0, 0);
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
        HealthController healthController = player.GetComponent<HealthController>();
        if (healthController != null)
        {
            healthController.SetArmourTier(armourTier); // Set the player's armour tier
        }
    }
}