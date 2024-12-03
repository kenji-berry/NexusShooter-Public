using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    public Recipe recipe;
    private InventoryManager inventoryManager;

    public TextMeshProUGUI recipeName;
    public Image icon;

    void Awake()
    {
        if (recipe != null)
        {
            recipeName.text = recipe.name;
            icon.sprite = recipe.result.itemData.icon;
        }

        inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (recipe == null) return;
        if (CanCraft())
        {
            CraftItem();
        }
    }

    private bool CanCraft()
    {
        foreach (ItemTypeAndCount itemTypeAndCount in recipe.requirements) 
        {
            if (!inventoryManager.HasItem(itemTypeAndCount.itemData, itemTypeAndCount.amount))
            {
                return false;
            }
        }
        return true;
    }

    private void CraftItem()
    {
        foreach (ItemTypeAndCount itemTypeAndCount in recipe.requirements)
        {
            inventoryManager.RemoveItem(itemTypeAndCount.itemData, itemTypeAndCount.amount);
        }

        inventoryManager.AddItem(recipe.result.itemData, recipe.result.amount);
    }
}
