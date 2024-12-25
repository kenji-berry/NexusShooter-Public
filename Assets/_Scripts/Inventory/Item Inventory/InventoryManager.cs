using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{ 
    public ItemInventorySlot[] slots = new ItemInventorySlot[9];
    public GameObject inventoryUI;

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
            gameObject.GetComponent<WeaponsManager>().isInventoryOpen = false;
        }
        else
        {
            inventoryUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.GetComponent<PlayerController>().inventoryOpen = true;
            gameObject.GetComponent<WeaponsManager>().isInventoryOpen = true;
        }
    }

    public bool PickUpItem(ItemInstance item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // if inventory contains item being picked up
            if (slots[i].item == item.itemData && slots[i].amount < slots[i].item.maxStackSize)
            {
                if (item.amount <= slots[i].item.maxStackSize - slots[i].amount)
                {
                    slots[i].amount += item.amount;
                    slots[i].UpdateSlot();
                    Destroy(item.gameObject);
                    return true;
                } else
                {
                    int remaining = slots[i].item.maxStackSize - item.amount;
                    slots[i].amount += slots[i].item.maxStackSize - item.amount;
                    slots[i].UpdateSlot();
                    item.amount = remaining;
                    return PickUpItem(item) || true;
                }
            } else if (slots[i].item == null)
            {
                slots[i].item = item.itemData;
                slots[i].amount += item.amount;
                slots[i].UpdateSlot();
                Destroy(item.gameObject);
                return true;
            }
        }

        return false;
    }

    public bool HasItem(ItemData item, int amount)
    {
        int itemCount = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                itemCount += slots[i].amount;
            }
        }

        return itemCount >= amount;
    }

    public void AddItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = item;
                slots[i].amount = amount;

                slots[i].UpdateSlot();
                return;
            }
        }
    }

    public void RemoveItem(ItemData item, int amount)
    {
        int leftToRemove = amount;

        if (HasItem(item, amount))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == item)
                {
                    if (leftToRemove >= slots[i].amount)
                    {
                        leftToRemove -= slots[i].amount;
                        slots[i].amount = 0;
                    } else 
                    {
                        slots[i].amount -= leftToRemove;
                        leftToRemove = 0;
                    }

                    slots[i].UpdateSlot();

                    if (leftToRemove == 0)
                    {
                        return;
                    }
                }
            }
        }
    }
}
