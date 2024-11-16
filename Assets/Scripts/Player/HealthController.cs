using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro namespace

public class HealthController : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public Slider healthBar;
    public TextMeshProUGUI armourText;

    // Armour tiers and their corresponding damage multipliers
    public enum ArmourTier { None, Light, Medium, Heavy }
    public ArmourTier currentArmourTier = ArmourTier.None;
    private Dictionary<ArmourTier, float> armourMultipliers = new Dictionary<ArmourTier, float>
    {
        { ArmourTier.None, 1.0f },
        { ArmourTier.Light, 0.75f },
        { ArmourTier.Medium, 0.5f },
        { ArmourTier.Heavy, 0.25f }
    };

    void Start()
    {
        currentHealth = maxHealth;

        // Initialize the health bar with the player's starting health
        UpdateHealthBar(currentHealth, maxHealth);
        UpdateArmourText(); // Initialize the armour text
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        float damageMultiplier = armourMultipliers[currentArmourTier];
        int adjustedDamage = Mathf.RoundToInt(damage * damageMultiplier);

        currentHealth -= adjustedDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar(currentHealth, maxHealth);
    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthBar(currentHealth, maxHealth);
    }

    // Method to change armour tier
    public void SetArmourTier(ArmourTier newArmourTier)
    {
        currentArmourTier = newArmourTier;
        UpdateArmourText(); // Update the armour text when the tier changes
    }

    // Method to update the armour text
    private void UpdateArmourText()
    {
        armourText.text = "Armour: " + currentArmourTier.ToString();
    }
}
