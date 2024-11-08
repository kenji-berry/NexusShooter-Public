using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot{
    [SerializeField] private ItemData item;
    [SerializeField] private int stackSize;

    public ItemData Item => item;
    public int Amount => stackSize;

    public InventorySlot(ItemData item, int amount){
        this.item = item;
        this.stackSize = amount;
    }

    public InventorySlot(){
        ClearSlot();
    }

    public void ClearSlot(){
        item = null;
        stackSize = -1;
    }

    public void UpdateInventorySlot(ItemData item, int amount){
        this.item = item;
        this.stackSize = amount;
    }

    public bool RoomLeftInStack(int amount){
        return this.stackSize + amount <= item.MaxStackSize;
    }

public bool RoomLeftInStack(int amount, out int amountRemaining){
        amountRemaining = item.MaxStackSize - this.stackSize;
        return RoomLeftInStack(amount);
    }

    public void AddToStack(int amount){
        this.stackSize += amount;

    }

    public void RemoveFromStack(int amount){
        this.stackSize -= amount;
    }

}
