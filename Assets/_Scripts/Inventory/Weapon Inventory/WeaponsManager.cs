using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class WeaponsManager : MonoBehaviour
{
    private AmmoManager ammoManager;

    const int WEAPON_INVENTORY_SIZE = 5;

    public List<Weapon> startingWeapons = new List<Weapon>();
    public WeaponSlot[] weaponSlots = new WeaponSlot[WEAPON_INVENTORY_SIZE];

    public GameObject inventoryUI;
    public TextMeshProUGUI weaponName;
    public Transform weaponHolder;

    private int selectedWeapon = -1;
    public bool isInventoryOpen = false;

    [Header("Ammo Panel")]
    public GameObject ammoPanel;
    public Image ammoIcon;
    public TextMeshProUGUI ammoText;

    [Header("Ammo Icons")]
    public Sprite bulletSprite;
    public Sprite shellSprite;
    public Sprite grenadeSprite;

    private Weapon[] gunSlots = new Weapon[WEAPON_INVENTORY_SIZE];

    void OnSelectWeaponOne(InputValue value) { SwitchWeapon(0); }

    void OnSelectWeaponTwo(InputValue value) { SwitchWeapon(1); }

    void OnSelectWeaponThree(InputValue value) { SwitchWeapon(2); }

    void OnSelectWeaponFour(InputValue value) { SwitchWeapon(3); }

    void OnSelectWeaponFive(InputValue value) { SwitchWeapon(4); }

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
        foreach (Weapon weapon in startingWeapons)
        {
            AddWeapon(weapon);
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
        if (selectedWeapon == -1)
        {
            selectedWeapon = pos;
            gunSlots[selectedWeapon].gameObject.SetActive(true);
            weaponSlots[selectedWeapon].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);

            // show ammo count
            ammoPanel.SetActive(true);
        }
        else
        {
            gunSlots[selectedWeapon].gameObject.SetActive(false);
            weaponSlots[selectedWeapon].GetComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f);

            selectedWeapon = pos;
            gunSlots[selectedWeapon].gameObject.SetActive(true);
            weaponSlots[selectedWeapon].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }

        weaponName.text = gunSlots[selectedWeapon].GetComponent<Weapon>().weaponData.weaponName;

        UpdateAmmoPanel();
    }

    void OnAttack(InputValue value)
    {
        if (selectedWeapon == -1 || isInventoryOpen || FindFirstObjectByType<GameController>().isPaused)
        {
            return;
        }

        Weapon weapon = gunSlots[selectedWeapon];
        if (value.isPressed)
        {
            weapon.BeginAttacking();
        }
        else
        {
            weapon.StopAttacking();
        }
    }

    public bool AddWeapon(Weapon weaponPrefab)
    {
        foreach (Weapon weapon in gunSlots) {
            if (weapon != null && weapon.GetComponent<Weapon>().weaponData.weaponName == weaponPrefab.gameObject.GetComponent<Weapon>().weaponData.weaponName)
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
        Weapon weaponInstance = Instantiate(weaponPrefab, weaponHolder);
        weaponInstance.playerCamera = Camera.main;

        switch (weaponPrefab.GetComponent<Weapon>().weaponData.weaponType)
        {
            case WeaponData.WeaponType.MELEE:
                break;

            case WeaponData.WeaponType.GUN:
                weaponInstance.GetComponent<Gun>().ammoText = ammoText;
                break;
        }

        weaponInstance.gameObject.SetActive(false);
        gunSlots[pos] = weaponInstance;

        // Add to UI inventory
        weaponSlots[pos].item = weaponPrefab.gameObject.GetComponent<ItemInstance>().itemData;
        if (weaponInstance.GetComponent<Weapon>().weaponData.weaponType == WeaponData.WeaponType.GUN)
        {
            weaponSlots[pos].ammoCountText.text = ammoManager.GetAmmo(weaponInstance.GetComponent<Gun>().gunData.ammoType).ToString();
        }

        /*else
        {
            gunSlots[pos].ammoCountText.text = "";
        }
        */

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
        if (gunSlots[selectedWeapon].GetComponent<Weapon>().weaponData.weaponType == WeaponData.WeaponType.MELEE)
        {
            ammoPanel.SetActive(false);
            return;
        }

        ammoPanel.SetActive(true);
        ammoText.text = ammoManager.GetAmmo(gunSlots[selectedWeapon].gameObject.GetComponent<Gun>().gunData.ammoType).ToString();

        switch (gunSlots[selectedWeapon].GetComponent<Gun>().gunData.ammoType)
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
            if (gunSlots[i] != null && gunSlots[i].GetComponent<Weapon>().weaponData.weaponType != WeaponData.WeaponType.MELEE)
            {
                weaponSlots[i].ammoCountText.text = ammoManager.GetAmmo(gunSlots[i].GetComponent<Gun>().gunData.ammoType).ToString();
            }
        }
    }
}