using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, shootableMask)) // Check if the raycast hits an enemy
        {
            EnemyHealthController enemyHealthController = hit.transform.GetComponent<EnemyHealthController>();
            if (enemyHealthController != null)
            {
                soundController.Play(soundController.hit);
                enemyHealthController.TakeDamage(gunData.damage);
            }
        }
    }
}
