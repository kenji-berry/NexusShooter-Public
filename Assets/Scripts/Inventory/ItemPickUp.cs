using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData itemData;

    private SphereCollider myCollider;


    private void Awake(){
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
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
