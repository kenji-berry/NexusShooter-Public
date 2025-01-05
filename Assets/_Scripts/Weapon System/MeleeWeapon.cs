using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : Weapon
{
    public int damage = 30;
    public float attackRange = 3f;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    public override void BeginAttacking()
    {
        transform.Rotate(0f, 60f, 0);
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
        transform.Rotate(0f, -60f, 0);
        return;
    }
}
