using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    public SoundController soundController;
    public Camera playerCamera;

    public int damage = 30;
    public float attackRange = 3f;

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    void OnMeleeAttack(InputValue value)
    {
        soundController.Play(soundController.melee, 0.1f);
        Attack();
    }

    void Attack()
    {
        Debug.Log("Performing melee attack");
        
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
}
