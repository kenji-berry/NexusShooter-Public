using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 10;
    private bool hasCollided = false; 
    // Start is called before the first frame update
    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return; // If the bullet has already collided, return
        print("Collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
        else{
            print("Collided with " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}
