using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private WeaponsManager weaponsManager;

    public int ammoCount = 30;

    void Start()
    {
        weaponsManager = FindFirstObjectByType<WeaponsManager>();

        if (weaponsManager == null)
        {
            Debug.LogError("Weapons manager not found");
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, 40 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponsManager.PickUpAmmo(ammoCount);
            Destroy(gameObject);
        }
    }
}
