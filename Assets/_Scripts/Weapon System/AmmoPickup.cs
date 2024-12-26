using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private AmmoManager ammoManager;

    public GunData.AmmoType type;
    public int ammoCount = 30;

    void Start()
    {
        ammoManager = FindFirstObjectByType<AmmoManager>();

        if (ammoManager == null)
        {
            Debug.LogError("Ammo manager not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ammoManager.AddAmmo(type, ammoCount);
            Destroy(gameObject);
        }
    }
}
