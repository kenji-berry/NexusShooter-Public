using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthController : MonoBehaviour
{
    public Enemy enemy; 
    public int maxHealth = 100;
    private int currentHealth;
    public int xpReward = 10; // Amount of XP to reward when this enemy is defeated
    public event System.Action<int> onDamageTaken; // event to notify subscribers when damage is taken

    public GameObject damageNumberPrefab; // Reference to the damage number prefab
    private Transform playerTransform; // Reference to the player's transform

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        onDamageTaken?.Invoke(damage); // Notify subscribers

        // Show damage number
        ShowDamageNumber(damage);

        if (currentHealth <= 0)
        {
            enemy.SetDead();
            gameObject.GetComponent<Collider>().enabled = false;

            // Reward XP when the enemy is defeated
            XPManager.instance.AddXP(xpReward);
        }
    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    // Method to show damage number when enemy is hit
    private void ShowDamageNumber(int damage)
    {
        if (damageNumberPrefab != null && playerTransform != null)
        {
            // Random offset for variance in positioning of number
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + randomOffset;

            GameObject damageNumber = Instantiate(damageNumberPrefab, spawnPosition, Quaternion.identity);
            TextMeshPro textMeshPro = damageNumber.GetComponentInChildren<TextMeshPro>();
            textMeshPro.text = damage.ToString();

            // Make the damage number face the player
            damageNumber.transform.LookAt(playerTransform);
            damageNumber.transform.Rotate(0, 180, 0); // Adjust rotation to face the player correctly

        }
        else
        {
            Debug.LogError("Damage number prefab or player transform is null.");
        }
    }
}
