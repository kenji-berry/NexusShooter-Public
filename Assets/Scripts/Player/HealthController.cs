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
    public TextMeshProUGUI healthText;
    public Slider armourBar;
    public TextMeshProUGUI armourText;

    public int armourDurability;
    public int maxDurability = 100;

    // Armour tiers and their corresponding damage multipliers
    public enum ArmourTier { None, Light, Medium, Heavy }
    public ArmourTier currentArmourTier = ArmourTier.None;
    public RectTransform healthBarFill;
    public RectTransform heathBarBackground;
    public RectTransform armourBarFill;
    public RectTransform armourBarBackground;
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
        UpdateHealthBar(currentHealth, maxHealth, 0);
        UpdateArmourUI(); // Initialize the armour text
    }

    // Method to update the health bar
    public void UpdateHealthBar(int currentHealth, int maxHealth, int amount)
    {
        if (amount > 0)
        {
            // Calculate the new width based on the amount added
            float newWidth = healthBarFill.sizeDelta.x + amount;
            float newBackgroundWidth = heathBarBackground.sizeDelta.x + amount;

            Debug.Log("New width: " + newWidth);
            Debug.Log("New background width: " + newBackgroundWidth);
            // Adjust the sizeDelta of the health bar fill and background
            healthBarFill.sizeDelta = new Vector2(newWidth, healthBarFill.sizeDelta.y);
            heathBarBackground.sizeDelta = new Vector2(newBackgroundWidth, heathBarBackground.sizeDelta.y);

            // Adjust the position to keep the health bar aligned
            Debug.Log("Amount: " + amount);
            healthBarFill.localPosition = new Vector3(healthBarFill.localPosition.x + (amount / 2), healthBarFill.localPosition.y, healthBarFill.localPosition.z);
            heathBarBackground.localPosition = new Vector3(heathBarBackground.localPosition.x + (amount / 2), heathBarBackground.localPosition.y, heathBarBackground.localPosition.z);
        }
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + "/" + maxHealth;
    }

    // Method to update the armour text
    public void UpdateArmourBar(int damage, int adjustedDamage, int maxDurability, int amount)
    {
        if (amount > 0)
        {
            // Calculate the new width based on the amount added
            float newWidth = armourBarFill.sizeDelta.x + amount;
            float newBackgroundWidth = armourBarBackground.sizeDelta.x + amount;

            Debug.Log("New width: " + newWidth);
            Debug.Log("New background width: " + newBackgroundWidth);
            // Adjust the sizeDelta of the armour bar fill and background
            armourBarFill.sizeDelta = new Vector2(newWidth, armourBarFill.sizeDelta.y);
            armourBarBackground.sizeDelta = new Vector2(newBackgroundWidth, armourBarBackground.sizeDelta.y);

            // Adjust the position to keep the armour bar aligned
            Debug.Log("Amount: " + amount);
            armourBarFill.localPosition = new Vector3(armourBarFill.localPosition.x + (amount / 2), armourBarFill.localPosition.y, armourBarFill.localPosition.z);
            armourBarBackground.localPosition = new Vector3(armourBarBackground.localPosition.x + (amount / 2), armourBarBackground.localPosition.y, armourBarBackground.localPosition.z);
        }
        armourDurability -= damage - adjustedDamage; // armour absorbs rest of damage
        armourBar.maxValue = maxDurability;
        armourBar.value = armourDurability;
        armourText.text = armourDurability + "/" + maxDurability;
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        float damageMultiplier = armourMultipliers[currentArmourTier];
        int adjustedDamage = Mathf.RoundToInt(damage * damageMultiplier);


        UpdateArmourBar(damage, adjustedDamage, maxDurability,0);
        Debug.Log("Damage: " + damage + " Adjusted Damage: " + adjustedDamage);

        if (armourDurability <= 0)
        {
            armourDurability = 0;
            currentArmourTier = ArmourTier.None;
            UpdateArmourUI();
        }

        currentHealth -= adjustedDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar(currentHealth, maxHealth, 0);

        if (currentHealth <= 0) 
        {
            FindFirstObjectByType<GameController>().Die();
            gameObject.GetComponent<PlayerController>().SetDead();
        }
    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthBar(currentHealth, maxHealth,0);
    }

    // Method to change armour tier
    public void SetArmourTier(ArmourTier newArmourTier)
    {
        currentArmourTier = newArmourTier;
        armourBar.value = armourDurability = 100;
        UpdateArmourUI(); // Update the armour text when the tier changes
    }

    // Method to increase max health
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Heal the player to full health
        UpdateHealthBar(currentHealth, maxHealth, amount);
    }

    // Method to increase armour durability
    public void IncreaseArmourDurability(int amount)
    {
        maxDurability += amount;
        armourDurability = maxDurability;
        UpdateArmourBar(0, 0, maxDurability, amount); // Pass dummy values for damage and adjustedDamage
        Debug.Log("Max durability increased. New max durability: " + maxDurability);
    }

    // Method to update the armour text
    private void UpdateArmourUI()
    {
        switch (currentArmourTier)
        {
            case ArmourTier.None:
                armourBar.gameObject.SetActive(false);
                return;

            case ArmourTier.Light:
                armourBar.fillRect.GetComponent<Image>().color = new Color(0.804f, 0.498f, 0.196f);
                armourBar.gameObject.SetActive(true);
                break;

            case ArmourTier.Medium:
                armourBar.fillRect.GetComponent<Image>().color = new Color(0.753f, 0.753f, 0.753f);
                armourBar.gameObject.SetActive(true);
                break;

            case ArmourTier.Heavy:
                armourBar.fillRect.GetComponent<Image>().color = new Color(1f, 0.843f, 0.0f);
                armourBar.gameObject.SetActive(true);
                break;
        }
    }
}
