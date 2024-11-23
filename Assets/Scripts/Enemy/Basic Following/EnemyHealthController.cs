using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;
    public delegate void DamageEvent(int damage); // delegate for damage event
    public event DamageEvent onDamageTaken; // event to notify subscribers when damage is taken

    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    // method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        onDamageTaken?.Invoke(damage); // notify subscribers

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
            GetComponent<EnemyAI>().isDead = true;
            //Destroy(gameObject);
        }
    }

    // method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }
}
