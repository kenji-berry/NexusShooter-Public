using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;

public class InventorySystem : MonoBehaviour
{ 
    [SerializeField] public InventorySlot[] slots = new InventorySlot[20];
    [SerializeField] public GameObject inventoryUI;

    public UnityAction<InventorySlot> OnInventorySlotUpdated;

    void Awake()
    {
        inventoryUI.SetActive(false);
    }

    void OnToggleInventory(InputValue value)
    {
        if (inventoryUI.activeInHierarchy)
        {
            inventoryUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.GetComponent<PlayerController>().inventoryOpen = false;
        }
        else
        {
            inventoryUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.GetComponent<PlayerController>().inventoryOpen = true;
        }
    }

    public void Add(Item item)
    {
        for (int i=0; i<slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item.itemData;
                slots[i].amount = item.amount;

                slots[i].UpdateSlot();
                Destroy(item.gameObject);
                return;
            }
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
            /*
            freeSlot.UpdateInventorySlot(item, amount);
            OnInventorySlotUpdated?.Invoke(freeSlot);
            */
            return true;
        }

        return false;
    }

    private int CountItemsOfType(ItemType itemType)
    {
        return slots.Count(slot => slot.item != null && slot.item.type == itemType);
    }

    public bool ContainsItem(ItemData item, out List<InventorySlot> invSlot){
        invSlot = slots.Where(x => x.item == item).ToList();
        return invSlot.Count > 0;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot){
        freeSlot = slots.FirstOrDefault(x => x.item == null);
        return freeSlot != null;
    }

}
