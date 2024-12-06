using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoveredItem : MonoBehaviour
{
    public ItemData item;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    void OnEnable()
    {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
    }
}
