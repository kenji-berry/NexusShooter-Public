using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] protected InventorySystem inventorySystem;

    [SerializeField] protected InventorySystem weaponInventorySystem;
    [SerializeField] private int inventorySize;

    [SerializeField] private int weaponInventorySize; // Size limit for weapon inventory


    public InventorySystem InventorySystem => inventorySystem;
    public InventorySystem WeaponInventorySystem => weaponInventorySystem; // Public getter for weapon inventory

    
    public static UnityAction<InventorySystem, InventorySystem> OnDynamicInventoryDisplayRequested;



    private void Awake(){
        inventorySystem = new InventorySystem(inventorySize);
        weaponInventorySystem = new InventorySystem(weaponInventorySize); 
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Trigger the inventory display request
            OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem, weaponInventorySystem);
        }
    }
}
