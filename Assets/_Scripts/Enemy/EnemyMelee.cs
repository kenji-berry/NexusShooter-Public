using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
    public SoundController soundController;
    void Awake()
    {
        damage = 10;
        attackRange = 2f;
    }

    public override void Attack()
    {
        agent.SetDestination(transform.position);

        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("isRunning", false);
            animator.SetTrigger("punch");

            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetComponent<HealthController>().TakeDamage(damage);
                soundController.Play(soundController.getHit, 0.5f);
            }

            nextAttackTime = Time.time + attackCooldown; // Set next attack time
        }
    }

    
}