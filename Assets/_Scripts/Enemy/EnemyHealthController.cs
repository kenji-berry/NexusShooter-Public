using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthController : MonoBehaviour
{
    private Enemy enemy;
    private Transform playerTransform;
    public int maxHealth = 100;
    public int currentHealth;
    public int xpReward = 50; // Amount of XP to reward when this enemy is defeated
    public event System.Action<int> onDamageTaken; // event to notify subscribers when damage is taken
    public GameObject bloodSprayPrefab;

    public GameObject damageNumberPrefab; // Reference to the damage number prefab
    public int critChance = 5; // 5% chance for a critical hit
    public int critMultiplier = 150;  // 1.5x damage for a critical hit
    
    public SoundController soundController;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    // Method to take damage
    public void TakeDamage(int damage)
    {
        // Check for critical hit if random value rolls a number less than critChance
        int randomValue = Random.Range(0, 100);
        bool isCrit = randomValue < critChance;
        Debug.Log("Is crit: " + isCrit);
        Debug.Log(critChance);
        if (isCrit)
        {
            damage = (critMultiplier * damage)/100; 
            soundController.Play(soundController.criticalHit, 0.5f);
        }
        Debug.Log("Damage: " + damage);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        onDamageTaken?.Invoke(damage); // Notify subscribers

        // Spawn blood effect   
        SpawnBloodEffect(transform.position, Vector3.up, isCrit);

        // Show damage number
        ShowDamageNumber(damage, isCrit);

        if (currentHealth <= 0)
        {
            enemy.Die();
            
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
    private void ShowDamageNumber(int damage, bool isCrit)
    {
        if (damageNumberPrefab != null && playerTransform != null)
        {
            // Random offset for variance in positioning of number
            Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + randomOffset;

            GameObject damageNumber = Instantiate(damageNumberPrefab, spawnPosition, Quaternion.identity);
            DamageNumber damageNumberScript = damageNumber.GetComponent<DamageNumber>();
            if (damageNumberScript != null)
            {
                damageNumberScript.Setup(isCrit);
            }
            TextMeshPro textMeshPro = damageNumber.GetComponentInChildren<TextMeshPro>();
            textMeshPro.text = damage.ToString();
            textMeshPro.color = isCrit ? Color.yellow : Color.white;

            // Make the damage number face the player
            damageNumber.transform.LookAt(playerTransform);
            damageNumber.transform.Rotate(0, 180, 0); // Adjust rotation to face the player correctly

        }
        else
        {
            Debug.LogError("Damage number prefab or player transform is null.");
        }
    }

    void SpawnBloodEffect(Vector3 hitPosition, Vector3 hitNormal, bool isCrit)
    {
        // Instantiate the blood spray at the hit position
        GameObject bloodSpray = Instantiate(bloodSprayPrefab, hitPosition, Quaternion.LookRotation(hitNormal));
        bloodSpray.layer = LayerMask.NameToLayer("Default");
        Destroy(bloodSpray, 2f);
    }

    public void increaseCritChance(int amount)
    {
        critChance = amount;
    }

    public void increaseCritDamage(int amount)
    {
        critMultiplier = amount;
    }

}
