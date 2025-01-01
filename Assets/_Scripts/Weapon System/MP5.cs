using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP5 : Gun
{
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity))
        { 
            EnemyHealthController enemyHealthController = hit.transform.GetComponent<EnemyHealthController>();
            if (enemyHealthController != null)
            {
                soundController.Play(soundController.hit);
                enemyHealthController.TakeDamage(gunData.damage);
            } else
            {
                GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 0.5f);
            }
        }
    }
}
