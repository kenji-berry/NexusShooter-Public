using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity))
        {
            // Check for enemy
            EnemyHealthController enemyHealthController = hit.transform.GetComponent<EnemyHealthController>();
            if (enemyHealthController != null)
            {
                soundController.Play(soundController.hit);
                enemyHealthController.TakeDamage(gunData.damage);
                return;
            }

            // Check for barrel
            ExplodingBarrel barrel = hit.transform.GetComponent<ExplodingBarrel>();
            if (barrel != null)
            {
                soundController.Play(soundController.hit);
                barrel.TakeDamage(gunData.damage);
            }
        }
    }
}
