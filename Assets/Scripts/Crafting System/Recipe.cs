using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting System/Recipe", order = 0)]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public List<ItemTypeAndCount> requirements;
    public ItemTypeAndCount result;
}

[System.Serializable]
public class ItemTypeAndCount
{
    public ItemData itemData;
    public int amount;

    public ItemTypeAndCount(ItemData itemData, int amount)
    {
        this.itemData = itemData;
        this.amount = amount;
    }
}
