using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public int pelletsPerShot = 20;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);

    public override void Shoot()
    {
        EnemyHealthController enemyHealthController = null;
        bool enemyHit = false;
        int totalDamage = 0;

        for (int i=0; i<pelletsPerShot; i++)
        {
            RaycastHit hit;
            Vector3 direction = GetDirection();
            if (Physics.Raycast(playerCamera.transform.position, direction, out hit, Mathf.Infinity))
            {
                if (enemyHealthController == null)
                {
                    enemyHealthController = hit.transform.GetComponent<EnemyHealthController>();
                }

                if (enemyHealthController != null)
                {
                    totalDamage += gunData.damage;
                    enemyHit = true;
                }
            }
        }

        if (enemyHit)
        {
            enemyHealthController.TakeDamage(totalDamage);
            soundController.Play(soundController.hit);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = playerCamera.transform.forward;

        direction += new Vector3(
            UnityEngine.Random.Range(-spread.x, spread.x),
            UnityEngine.Random.Range(-spread.y, spread.y),
            UnityEngine.Random.Range(-spread.z, spread.z)
        );

        direction.Normalize();

        return direction;
    }
}
