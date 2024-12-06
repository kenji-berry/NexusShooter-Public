using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoveredItemUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.GetComponentInParent<ItemInventorySlot>().item == null || hoveredItemUI.activeInHierarchy)
        {
            return;
        }

        hoveredItemUI.transform.position = eventData.pointerEnter.gameObject.transform.parent.transform.position + new Vector3(
            0, -(hoveredItemUI.GetComponent<RectTransform>().rect.height / 2), 0);
        hoveredItemUI.GetComponent<HoveredItem>().item = eventData.pointerEnter.GetComponentInParent<ItemInventorySlot>().item;
        hoveredItemUI.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoveredItemUI.SetActive(false);
    }
}
