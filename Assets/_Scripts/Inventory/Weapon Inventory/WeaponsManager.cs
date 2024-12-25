using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class WeaponsManager : MonoBehaviour
{
    private AmmoManager ammoManager;

    const int WEAPON_INVENTORY_SIZE = 3;

    public List<Gun> startingGuns = new List<Gun>();
    public WeaponSlot[] weaponSlots = new WeaponSlot[WEAPON_INVENTORY_SIZE];

    public GameObject inventoryUI;
    public TextMeshProUGUI weaponName;
    public Transform weaponHolder;

    private int selectedGun = -1;
    public bool isInventoryOpen = false;

    [Header("Ammo Panel")]
    public GameObject ammoPanel;
    public Image ammoIcon;
    public TextMeshProUGUI ammoText;

    [Header("Ammo Icons")]
    public Sprite bulletSprite;
    public Sprite shellSprite;
    public Sprite grenadeSprite;

    private Gun[] gunSlots = new Gun[WEAPON_INVENTORY_SIZE];

    void OnSelectWeaponOne(InputValue value) { SwitchWeapon(0); }

    void OnSelectWeaponTwo(InputValue value) { SwitchWeapon(1); }

    void OnSelectWeaponThree(InputValue value) { SwitchWeapon(2); }

    void Awake()
    {
        ammoManager = GameObject.FindFirstObjectByType<AmmoManager>();
        if (ammoManager == null)
        {
            Debug.LogError("Ammo manager not found.");
        }
    }

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

    public void SwitchWeapon(int pos)
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
            weaponSlots[selectedGun].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);

            // show ammo count
            ammoPanel.SetActive(true);
        }
        else
        {
            gunSlots[selectedGun].gameObject.SetActive(false);
            weaponSlots[selectedGun].GetComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f);

            selectedGun = pos;
            gunSlots[selectedGun].gameObject.SetActive(true);
            weaponSlots[selectedGun].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }

        weaponName.text = gunSlots[selectedGun].GetComponent<Gun>().gunData.gunName;

        UpdateAmmoPanel();
    }

    void OnShoot(InputValue value)
    {

        if (selectedGun == -1 || isInventoryOpen || FindFirstObjectByType<GameController>().isPaused)
        {
            return;
        }

        Gun gun = gunSlots[selectedGun];
        if (value.isPressed)
        {
            gun.StartShooting();
        }
        else
        {
            gun.StopShooting();
        }
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
        gunInstance.ammoText = ammoText;
        gunInstance.gameObject.SetActive(false);
        gunSlots[pos] = gunInstance;

        // Add to UI inventory
        weaponSlots[pos].item = gunPrefab.gameObject.GetComponent<ItemInstance>().itemData;
        weaponSlots[pos].ammoCountText.text = ammoManager.GetAmmo(gunInstance.GetComponent<Gun>().gunData.ammoType).ToString();
        weaponSlots[pos].UpdateSlot();

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

    public void UpdateAmmoPanel()
    {
        ammoText.text = ammoManager.GetAmmo(gunSlots[selectedGun].gameObject.GetComponent<Gun>().gunData.ammoType).ToString();

        switch (gunSlots[selectedGun].GetComponent<Gun>().gunData.ammoType)
        {
            case GunData.AmmoType.BULLETS:
                ammoIcon.sprite = bulletSprite;
                break;

            case GunData.AmmoType.SHELLS:
                ammoIcon.sprite = shellSprite;
                break;

            case GunData.AmmoType.GRENADES:
                ammoIcon.sprite = grenadeSprite;
                break;
        }
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < gunSlots.Length; i++)
        {
            if (gunSlots[i] != null)
            {
                weaponSlots[i].ammoCountText.text = ammoManager.GetAmmo(gunSlots[i].GetComponent<Gun>().gunData.ammoType).ToString();
            }
        }
    }
}