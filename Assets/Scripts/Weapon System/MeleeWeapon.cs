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

<<<<<<< HEAD:Assets/Scripts/Weapon System/MeleeWeapon.cs
    public void Attack()
=======
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
>>>>>>> 34b9e90284fa343abb1140a5b18e2d2b463a3a0d:Assets/Scripts/Player/Weapon System/MeleeWeapon.cs
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
