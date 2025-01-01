using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public enum WeaponType
    {
        MELEE,
        GUN
    }

    public WeaponType weaponType;
}
