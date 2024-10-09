using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int itemIndex = 1; // What item is it? 1 = health, 2 = ammo, etc.

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
    }
}