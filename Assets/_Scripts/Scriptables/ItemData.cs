using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Inventory System/Item", order = 0)]
public class ItemData : ScriptableObject 
{
    public int id;
    public string itemName;
    [TextArea(3, 3)] public string description;
    public Sprite icon;

    public GameObject prefab;

    public int maxStackSize;
}