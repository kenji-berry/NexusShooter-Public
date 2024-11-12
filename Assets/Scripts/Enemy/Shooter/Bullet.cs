using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    private bool hasCollided = false;
    
    void Start()
    {
        // disable collisions with enemy layer
        Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null) 
            {
                playerController.GetComponent<HealthController>().TakeDamage(damage);
                Debug.Log("Health is " + playerController.GetComponent<HealthController>().currentHealth);
            }
            hasCollided = true;
            Destroy(gameObject);
        }
    }
}
