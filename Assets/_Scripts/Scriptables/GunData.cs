using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public enum ShootingType
    {
        Single,
        Auto,
        SemiAuto // e.g., burst fire
    }

    public enum AmmoType
    {
        BULLETS,
        SHELLS,
        GRENADES
    }

    public AmmoType ammoType;

    public int maxAmmo;

    public float fireRate;

    public int damage;

    public AudioClip shootSound;

    public ShootingType shootingType; // New field for firing mode
    public int burstCount = 3; // Used for SemiAuto guns
}
