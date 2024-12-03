using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlot : MonoBehaviour
{
    public ItemTypeAndCount itemTypeAndCount;

    public Image icon;
    public TextMeshProUGUI ingredientName;
    public TextMeshProUGUI amount;

    public void UpdateSlot()
    {
        icon.sprite = itemTypeAndCount.itemData.icon;
        ingredientName.text = itemTypeAndCount.itemData.itemName;
        amount.text = $"x{itemTypeAndCount.amount}";
    }
}
