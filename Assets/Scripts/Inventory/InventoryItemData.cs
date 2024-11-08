using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu( menuName = "Inventory System/Inventory Item", order = 0)]
public class InventoryItemData : ScriptableObject {

    public int ID;
    public string DisplayName;
    [TextArea(15, 20)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;

    public ItemType Type; // New field to specify item type

}
