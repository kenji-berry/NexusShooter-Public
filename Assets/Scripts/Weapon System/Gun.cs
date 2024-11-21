using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammoText;

    private float nextTimeToFire = 0f;
    public int currentAmmo;

    public SoundController soundController;

    protected int shootableMask; // Layer mask for objects we can shoot

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
        // Include both Default and Enemy layers
        shootableMask = LayerMask.GetMask("Default", "Enemy");
        
        // Debug log to verify layer mask
        Debug.Log($"Shootable Layer Mask: {shootableMask}");

        currentAmmo = gunData.maxAmmo;
    }

    public void TryShoot()
    {
        if (Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }
    }

    public void HandleShoot()
    {
        muzzleFlash.Play();
        soundController.Play(gunData.shootSound);
        Shoot();
        currentAmmo--;
        UpdateAmmoUI();
    }

    // Optional: Add debug visualization of raycast
    protected void DebugRaycast(RaycastHit hit, bool didHit)
    {
        if (didHit)
        {
            Debug.DrawLine(playerCamera.transform.position, hit.point, Color.green, 1f);
            Debug.Log($"Hit object: {hit.transform.name} on layer: {hit.transform.gameObject.layer}");
        }
        else
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 100f, Color.red, 1f);
            Debug.Log("No hit detected");
        }
    }

    private void UpdateAmmoUI()
    {
        //ammoText.text = currentAmmo.ToString();


        GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>().text = currentAmmo.ToString();
    }

    public abstract void Shoot();
}
