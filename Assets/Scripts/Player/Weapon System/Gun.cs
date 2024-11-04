using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;

    public bool isShooting = false;
    public bool readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 0.1f;
    public float spread;

    public enum ShootingMode {
        SINGLE, AUTO
    }

    public ShootingMode shootingMode;

    public float bulletVelocity = 300f;
    public float bulletLifeTime = 3f;

    public int damage = 30;

    public SoundController soundController;

    void OnShoot(InputValue value)
    {
        HandleShoot();
    }

    void Awake()
    {
        readyToShoot = true;
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    void Update()
    {
        if(shootingMode == ShootingMode.AUTO){
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }

        if(readyToShoot && isShooting){
            HandleShoot();
        }
    }

    public void HandleShoot()
    {
        if (readyToShoot)
        {
            muzzleFlash.Play();
            soundController.Play(soundController.shoot);
            readyToShoot = false;

            Shoot();

            if (allowReset)
            {
                Invoke("ResetShot", shootingDelay);
                allowReset = false;
            }
        }
    }

    public abstract void Shoot();

    public void ToggleShootMode()
    {
        switch (shootingMode)
        {
            case ShootingMode.SINGLE:
                shootingMode = ShootingMode.AUTO; break;

            case ShootingMode.AUTO:
                shootingMode = ShootingMode.SINGLE; break;
        }
    }

    private void ResetShot(){
        readyToShoot = true;
        allowReset = true;
    }
}
