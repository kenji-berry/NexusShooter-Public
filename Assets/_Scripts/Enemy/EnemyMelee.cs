using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();

        damage = 10;
        attackRange = 2f;
        attackCooldown = 2f;
    }

    public override void Attack()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.GetComponent<HealthController>().TakeDamage(damage);
            soundController.Play(soundController.getHit, 0.5f);
        }
    }

    
}