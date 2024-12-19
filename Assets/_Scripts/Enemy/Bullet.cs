using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Debug.Log("enemy");
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null) 
            {
                playerController.GetComponent<HealthController>().TakeDamage(damage);
                Debug.Log("Health is " + playerController.GetComponent<HealthController>().currentHealth);
            }
        } 
        Destroy(gameObject);
    }
}
