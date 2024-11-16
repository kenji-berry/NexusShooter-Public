using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMesh Pro namespace

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab; // Prefab for each inventory slot
    [SerializeField] private Transform slotContainer; // Content area in ScrollView

    [SerializeField] private GameObject weaponSlotPrefab; // Prefab for each inventory slot
    [SerializeField] private Transform weaponSlotContainer; // Content area in ScrollView

    [SerializeField] private GameObject inventoryPanel; // Panel that holds the inventory UI


   private bool isInventoryOpen = false; // Track inventory open state
    private Dictionary<InventorySlot, GameObject> slotToGameObjectMap = new Dictionary<InventorySlot, GameObject>();

    private void Awake(){
        inventoryPanel.SetActive(isInventoryOpen);

    }
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }

        private void EnableCursor(){
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

        private void DisableCursor(){
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }

    private void DisplayInventory(InventorySystem inventorySystem, InventorySystem weaponInventorySystem)
    {
        isInventoryOpen = !isInventoryOpen;
        if(isInventoryOpen){
            EnableCursor();
        }else{
            DisableCursor();
        }
        inventoryPanel.SetActive(isInventoryOpen);
        Debug.Log("Displaying inventory");
        Debug.Log(isInventoryOpen);
        ClearExistingSlots();
        foreach (var slot in inventorySystem.slots)
        {
            var slotUI = Instantiate(slotPrefab, slotContainer);
            slotToGameObjectMap[slot] = slotUI;
            UpdateSlotUI(slot); // Initial update for each slot
        }

        foreach (var slot in weaponInventorySystem.slots)
        {
            var slotUI = Instantiate(weaponSlotPrefab, weaponSlotContainer);
            slotToGameObjectMap[slot] = slotUI;
            UpdateSlotUI(slot); // Initial update for each slot
        }

        inventorySystem.OnInventorySlotUpdated += UpdateSlotUI;
        weaponInventorySystem.OnInventorySlotUpdated += UpdateSlotUI;
    }

    private void UpdateSlotUI(InventorySlot slot)
    {
        if (slotToGameObjectMap.TryGetValue(slot, out var slotUI))
        {
            var itemIcon = slotUI.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCount = slotUI.transform.Find("ItemCount").GetComponent<TMP_Text>();
            var itemTitle = slotUI.transform.Find("ItemTitle").GetComponent<TMP_Text>();
            if (slot.item != null)
            {
                slotUI.SetActive(true);
                itemIcon.sprite = slot.item.icon;
                itemIcon.enabled = true;
                itemTitle.text = slot.item.name;
                itemCount.text = slot.amount.ToString();
            }
            else
            {
                slotUI.SetActive(false);
            }
        }
    }

    private void ClearExistingSlots()
    {
        foreach (var slotUI in slotToGameObjectMap.Values)
        {
            Destroy(slotUI);
        }
        slotToGameObjectMap.Clear();
    }
}
