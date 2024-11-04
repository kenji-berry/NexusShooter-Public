using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public override void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit))
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
