using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponsManager : MonoBehaviour
{
    public List<Gun> startingGuns = new List<Gun>();

    public Transform weaponHolder;

    public int selectedGun = -1;

    public Gun[] gunSlots = new Gun[3];

    void Start()
    {
        foreach (Gun gun in startingGuns)
        {
            AddGun(gun);
        }
    }

    void OnSelectWeaponOne(InputValue value)
    {
        SwitchWeapon(0);
        Debug.Log("Switching to weapon 0");
    }

    void OnSelectWeaponTwo(InputValue value)
    {
        SwitchWeapon(1);
        Debug.Log("Switching to weapon 1");
    }

    void OnSelectWeaponThree(InputValue value)
    {
        SwitchWeapon(2);
        Debug.Log("Switching to weapon 2");
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
        }
        else
        {
            gunSlots[selectedGun].gameObject.SetActive(false);
            selectedGun = pos;
            gunSlots[selectedGun].gameObject.SetActive(true);
        }
    }

    void OnShoot(InputValue value)
    {
        if (selectedGun == -1)
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

        SwitchWeapon(pos);
        return true;
    }

    int FindNextFreeSlot()
    {
        for (int i=0; i<gunSlots.Length; i++) 
        { 
            if (gunSlots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}