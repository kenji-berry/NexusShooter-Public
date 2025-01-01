using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{
    public int damage = 30;
    public float attackRange = 3f;

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    public override void BeginAttacking()
    {
        soundController.Play(soundController.melee);
        
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, attackRange))
        {
            EnemyHealthController enemyHealthController = hit.collider.GetComponent<EnemyHealthController>();
            if (enemyHealthController != null)
            {
                enemyHealthController.TakeDamage(damage);
            }
        }
    }

    public override void StopAttacking()
    {
        return;
    }
}
