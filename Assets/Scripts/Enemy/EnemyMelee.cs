using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Enemy
{
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
            }

            nextAttackTime = Time.time + attackCooldown; // Set next attack time
        }
    }

    
}