using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class Gun : Weapon
{
    public GunData gunData;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;

    private float nextTimeToFire = 0f;

    private AmmoManager ammoManager;
    public TextMeshProUGUI ammoText;

    private bool isShooting = false;

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
        ammoManager = GameObject.FindFirstObjectByType<AmmoManager>();
    }

    public override void BeginAttacking()
    {
        isShooting = true;

        switch (gunData.shootingType)
        {
            case GunData.ShootingType.Single:
                TryShoot();
                break;

            case GunData.ShootingType.Auto:
                StartCoroutine(AutoShoot());
                break;

            case GunData.ShootingType.SemiAuto:
                StartCoroutine(BurstShoot());
                break;
        }
    }
        
    public override void StopAttacking()
    {
        isShooting = false; // Stop any ongoing shooting
    }

    private IEnumerator AutoShoot()
    {
        while (isShooting && ammoManager.GetAmmo(gunData.ammoType) > 0)
        {
            TryShoot();
            yield return new WaitForSeconds(1 / gunData.fireRate);
        }
    }
    
    private IEnumerator BurstShoot()
    {
        int shotsFired = 0;

        while (shotsFired < gunData.burstCount && ammoManager.GetAmmo(gunData.ammoType) > 0)
        {
            TryShoot();
            shotsFired++;
            yield return new WaitForSeconds(1 / gunData.fireRate);
        }
    }

    public void TryShoot()
    {
        if (Time.time >= nextTimeToFire && ammoManager.GetAmmo(gunData.ammoType) > 0)
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
        ammoManager.UseAmmo(gunData.ammoType, 1);
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
        ammoText.text = ammoManager.GetAmmo(gunData.ammoType).ToString();
    }

    public abstract void Shoot();
}
