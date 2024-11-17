using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public string gunName;

    public float fireRate;

    public int damage;

    public AudioClip shootSound;
}
