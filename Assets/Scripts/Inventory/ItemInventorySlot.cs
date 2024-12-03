using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventorySlot : MonoBehaviour
{
    public ItemData item;
    public int amount;

    public Image icon;
    public TextMeshProUGUI amountText;

    public void UpdateSlot()
    {
        if (amount == 0)
        {
            ClearSlot();
        } else {
            icon.sprite = item.icon;
            icon.color = new Color(1f, 1f, 1f);
            amountText.text = $"{amount}x";
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.color = new Color(1f, 1f, 1f);
        amountText.text = "";
    }

    public bool RoomLeftInStack(int amount)
    {
        return this.amount + amount <= item.maxStackSize;
    }

    public bool RoomLeftInStack(int amount, out int amountRemaining)
    {
        amountRemaining = item.maxStackSize - this.amount;
        return RoomLeftInStack(amount);
    }

    public void AddToStack(int amount)
    {
        this.amount += amount;

    }

    public void RemoveFromStack(int amount)
    {
        this.amount -= amount;
    }
}
