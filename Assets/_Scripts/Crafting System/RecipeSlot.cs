using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    public Recipe recipe;
    private InventoryManager inventoryManager;
    public GameObject ingredientSlotPrefab;

    public TextMeshProUGUI recipeName;
    public Image icon;
    public GameObject ingredientPanel;

    void Awake()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public void UpdateSlot()
    {
        recipeName.text = recipe.name;
        icon.sprite = recipe.result.itemData.icon;

        foreach (ItemTypeAndCount itemTypeAndCount in recipe.requirements) 
        {
            GameObject newIngredientSlot = Instantiate(ingredientSlotPrefab, ingredientPanel.transform);
            newIngredientSlot.GetComponent<IngredientSlot>().itemTypeAndCount = itemTypeAndCount;
            newIngredientSlot.GetComponent<IngredientSlot>().UpdateSlot();
        }
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
