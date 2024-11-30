using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMesh Pro namespace

public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public static int xp = 0;
    public Slider xpSlider; // Reference to the XP Slider
    public TextMeshProUGUI levelText; // Reference to the Level Text
    public TextMeshProUGUI skillPointsText; // Reference to the Skill Points Text
    public HealthController healthController; // Reference to the HealthController script
    public TextMeshProUGUI upgradeButtonText; // Reference to the upgrade button text
    public TextMeshProUGUI upgradeCostText; // Reference to the upgrade cost text

    private int level = 1;
    private int xpToNextLevel = 10;
    private static int skillPoints = 0; // Skill points that the player can use

    // Define upgrade tiers
    private struct UpgradeTier
    {
        public int healthIncrease;
        public int skillPointsCost;

        public UpgradeTier(int healthIncrease, int skillPointsCost)
        {
            this.healthIncrease = healthIncrease;
            this.skillPointsCost = skillPointsCost;
        }
    }

    private List<UpgradeTier> upgradeTiers = new List<UpgradeTier>
    {
        new UpgradeTier(10, 1), // Tier 1: +10 health, 1 skill point
        new UpgradeTier(15, 2), // Tier 2: +15 health, 2 skill points
        new UpgradeTier(25, 3)  // Tier 3: +25 health, 3 skill points
    };

    private int currentUpgradeTier = 0; // Track the current upgrade tier

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = xp;
        }

        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }

        UpdateSkillPointsText();
        UpdateUpgradeButtonText();
    }

    public void AddXP(int amount)
    {
        xp += amount;
        Debug.Log("XP added: " + amount + ". Total XP: " + xp);
        CheckLevelUp();
        UpdateXPSlider();
    }

    public int GetXP()
    {
        return xp;
    }

    private void CheckLevelUp()
    {
        while (xp >= xpToNextLevel)
        {
            xp -= xpToNextLevel;
            level++;
            xpToNextLevel += 50; // Increase the XP required for the next level
            AwardSkillPoints();
            UpdateLevelText();
        }
    }

    private void AwardSkillPoints()
    {
        if (level <= 5)
        {
            skillPoints += 1;
        }
        else
        {
            skillPoints += 2;
        }
        Debug.Log("Skill points awarded. Total skill points: " + skillPoints);
        UpdateSkillPointsText();
    }

    private void UpdateXPSlider()
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNextLevel;
            xpSlider.value = xp;
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + level;
        }
    }

    private void UpdateSkillPointsText()
    {
        if (skillPointsText != null)
        {
            skillPointsText.text = "Skill Points: " + skillPoints;
        }
    }

    private void UpdateUpgradeButtonText()
    {
        if (currentUpgradeTier < upgradeTiers.Count)
        {
            UpgradeTier tier = upgradeTiers[currentUpgradeTier];
            if (upgradeButtonText != null)
            {
                upgradeButtonText.text = "Increase Health by " + tier.healthIncrease;
            }
            if (upgradeCostText != null)
            {
                upgradeCostText.text = "Cost: " + tier.skillPointsCost + " Skill Points";
            }
        }
        else
        {
            if (upgradeButtonText != null)
            {
                upgradeButtonText.text = "Max Upgrade Reached";
            }
            if (upgradeCostText != null)
            {
                upgradeCostText.text = "";
            }
        }
    }

    // Public method to get the current skill points
    public int GetSkillPoints()
    {
        return skillPoints;
    }

    // Public method to add skill points
    public void AddSkillPoints(int amount)
    {
        skillPoints += amount;
        Debug.Log("Skill points added: " + amount + ". Total skill points: " + skillPoints);
        UpdateSkillPointsText();
    }

    // Public method to use/subtract skill points
    public bool UseSkillPoints(int amount)
    {
        if (skillPoints >= amount)
        {
            skillPoints -= amount;
            Debug.Log("Skill points used: " + amount + ". Total skill points: " + skillPoints);
            UpdateSkillPointsText();
            return true;
        }
        else
        {
            Debug.Log("Not enough skill points. Total skill points: " + skillPoints);
            return false;
        }
    }

    // Method to use skill points to increase health
    public void UseSkillPointsForHealth()
    {
        if (currentUpgradeTier < upgradeTiers.Count)
        {
            UpgradeTier tier = upgradeTiers[currentUpgradeTier];
            if (UseSkillPoints(tier.skillPointsCost))
            {
                healthController.IncreaseMaxHealth(tier.healthIncrease);
                Debug.Log("Skill points used to increase health by " + tier.healthIncrease);
                currentUpgradeTier++; // Move to the next upgrade tier
                UpdateUpgradeButtonText(); // Update the button text and cost
            }
            else
            {
                Debug.Log("Not enough skill points to increase health.");
            }
        }
        else
        {
            Debug.Log("Max upgrade tier reached.");
        }
    }
}
