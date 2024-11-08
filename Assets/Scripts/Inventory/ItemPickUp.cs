using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;

    private float bounceHeight = 0.1f;
    private float bounceSpeed = 3f;
    private Vector3 originalPosition;

    public InventoryItemData itemData;

    private SphereCollider myCollider;

    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;

    }

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, 40 * Time.deltaTime, 0); // Rotate the item
        float newY = originalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight; // Move the item up and down
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z); // Set the new position
    }

    private void OnTriggerEnter(Collider other) {
        var inventory = other.transform.GetComponent<InventoryHolder>();
        if(!inventory) return;

        if(itemData.Type == ItemType.Weapon){
            if(inventory.WeaponInventorySystem.AddToInventory(itemData, 1)){
                Destroy(gameObject);
            }
        }
        else{
            if(inventory.InventorySystem.AddToInventory(itemData, 1)){
                Destroy(gameObject);
            }
        }


    }
}
