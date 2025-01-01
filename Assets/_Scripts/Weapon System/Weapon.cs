using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public Camera playerCamera;
    public SoundController soundController;

    public abstract void BeginAttacking();

    public abstract void StopAttacking();
}
