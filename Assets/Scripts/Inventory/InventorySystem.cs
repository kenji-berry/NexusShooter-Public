using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;

    public int InventorySize => inventorySlots.Count;


    public UnityAction<InventorySlot> OnInventorySlotUpdated;


    public InventorySystem(int size){
        inventorySlots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++){
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(ItemData item, int amount){

        // if (item.Type == ItemType.Weapon && CountItemsOfType(ItemType.Weapon) >= maxWeaponSlots)
        // {
        //     Debug.Log("Weapon slot limit reached!");
        //     return false; // Deny addition if weapon slot limit is reached
        // }
        // check if item exists in inventory
        if(ContainsItem(item, out List<InventorySlot> invSlot)){
            Debug.Log("Item exists in inventory");
            foreach (var slot in invSlot)
            {
                if(slot.RoomLeftInStack(amount)){
                    slot.AddToStack(amount);
                    OnInventorySlotUpdated?.Invoke(slot);
                    return true;
                }
            }
    
        }
        // gets free slot
        if(HasFreeSlot(out InventorySlot freeSlot)){
            Debug.Log("Has free slot");
            freeSlot.UpdateInventorySlot(item, amount);
            OnInventorySlotUpdated?.Invoke(freeSlot);
            return true;
        }

        return false;
    }

    private int CountItemsOfType(ItemType itemType)
    {
        return inventorySlots.Count(slot => slot.Item != null && slot.Item.Type == itemType);
    }

    public bool ContainsItem(ItemData item, out List<InventorySlot> invSlot){
        invSlot = inventorySlots.Where(x => x.Item == item).ToList();
        return invSlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot){
        freeSlot = inventorySlots.FirstOrDefault(x => x.Item == null);
        return freeSlot != null;
    }

}
