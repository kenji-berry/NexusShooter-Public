using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryUIController : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick.GetComponent<InventorySlot>().item != null)
        {
            Debug.Log(eventData.pointerClick.GetComponent<InventorySlot>().item.itemName);
        }
    }
}
