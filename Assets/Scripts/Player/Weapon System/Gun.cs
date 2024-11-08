using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;

    private float nextTimeToFire = 0f;

    public SoundController soundController;

    void OnShoot(InputValue value)
    {
        TryShoot();
    }

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    public void TryShoot()
    {
        if (Time.time >= nextTimeToFire)
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
    }

    public abstract void Shoot();
}
