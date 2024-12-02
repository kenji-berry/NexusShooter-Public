using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;

public class InventoryManager : MonoBehaviour
{ 
    [SerializeField] public ItemInventorySlot[] slots = new ItemInventorySlot[9];
    [SerializeField] public GameObject inventoryUI;

    public UnityAction<InventorySlot> OnInventorySlotUpdated;

    void Awake()
    {
        inventoryUI.SetActive(false);
    }

    void OnToggleItemInventory(InputValue value)
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

    public void PickUpItem(ItemInstance item)
    {
        for (int i = 0; i < slots.Length; i++)
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
}
