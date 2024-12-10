using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotUIInteraction : MonoBehaviour, IPointerClickHandler
{
    private WeaponsManager weaponsManager;

    public int slotIndex;

    void Start()
    {
        weaponsManager = FindFirstObjectByType<WeaponsManager>();

        if (weaponsManager == null )
        {
            Debug.LogError("Weapons manager not found.");
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        weaponsManager.SwitchWeapon(slotIndex);
    }
}
