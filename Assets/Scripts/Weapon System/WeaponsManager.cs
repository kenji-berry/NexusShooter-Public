using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class WeaponsManager : MonoBehaviour
{
    const int WEAPON_INVENTORY_SIZE = 3;

    public List<Gun> startingGuns = new List<Gun>();
    public InventorySlot[] inventorySlots = new InventorySlot[WEAPON_INVENTORY_SIZE];

    public GameObject inventoryUI;
    public Transform weaponHolder;

    public GameObject ammoPanel;
    public TextMeshProUGUI ammoText;

    public int selectedGun = -1;
    private bool isInventoryOpen = false;

    public Gun[] gunSlots = new Gun[WEAPON_INVENTORY_SIZE];

    void OnSelectWeaponOne(InputValue value) { SwitchWeapon(0); }

    void OnSelectWeaponTwo(InputValue value) { SwitchWeapon(1); }

    void OnSelectWeaponThree(InputValue value) { SwitchWeapon(2); }

    void Start()
    {
        foreach (Gun gun in startingGuns)
        {
            AddGun(gun);
        }
    }

    void OnToggleWeaponInventory(InputValue value)
    {
        if (value.isPressed)
        {
            UpdateInventoryUI();
            inventoryUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.GetComponent<PlayerController>().inventoryOpen = true;
            isInventoryOpen = true;
        } else
        {
            inventoryUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.GetComponent<PlayerController>().inventoryOpen = false;
            isInventoryOpen = false;
        }
    }

    void SwitchWeapon(int pos)
    {
        if (gunSlots[pos] == null)
        {
            return;
        }

        // if no guns exist yet 
        if (selectedGun == -1)
        {
            selectedGun = pos;
            gunSlots[selectedGun].gameObject.SetActive(true);
            inventorySlots[selectedGun].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);

            // show ammo count
            ammoPanel.SetActive(true);
        }
        else
        {
            gunSlots[selectedGun].gameObject.SetActive(false);
            inventorySlots[selectedGun].GetComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f);

            selectedGun = pos;
            gunSlots[selectedGun].gameObject.SetActive(true);
            inventorySlots[selectedGun].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }

        // initialise ammo text
        ammoText.text = gunSlots[selectedGun].gameObject.GetComponent<Gun>().currentAmmo.ToString();
    }

    void OnShoot(InputValue value)
    {

        if (selectedGun == -1 || isInventoryOpen || FindFirstObjectByType<GameController>().isPaused)
        {
            return;
        }

        Gun gun = gunSlots[selectedGun];
        gun.TryShoot();
    }

    public bool AddGun(Gun gunPrefab)
    {
        foreach (Gun gun in gunSlots) {
            if (gun != null && gun.GetComponent<Gun>().gunData.gunName == gunPrefab.gameObject.GetComponent<Gun>().gunData.gunName)
            {
                return false;
            }
        }

        int pos = FindNextFreeSlot();

        // return if there is no space in the inventory
        if (pos == -1)
        {
            return false;
        }

        // spawn a gun as a child of the weapon holder
        Gun gunInstance = Instantiate(gunPrefab, weaponHolder);
        gunInstance.playerCamera = Camera.main;
        gunInstance.gameObject.SetActive(false);
        gunSlots[pos] = gunInstance;

        // Add to UI inventory
        inventorySlots[pos].item = gunPrefab.gameObject.GetComponent<ItemInstance>().itemData;
        inventorySlots[pos].ammoCountText.text = gunInstance.GetComponent<Gun>().currentAmmo.ToString();
        inventorySlots[pos].UpdateSlot();

        SwitchWeapon(pos);
        return true;
    }

    int FindNextFreeSlot()
    {
        for (int i=0; i<WEAPON_INVENTORY_SIZE; i++) 
        { 
            if (gunSlots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < gunSlots.Length; i++)
        {
            if (gunSlots[i] != null)
            {
                inventorySlots[i].ammoCountText.text = gunSlots[i].GetComponent<Gun>().currentAmmo.ToString();
            }
        }
    }
}