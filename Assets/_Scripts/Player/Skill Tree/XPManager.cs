using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public TextMeshProUGUI levelUpText; // Reference to the Level Up Text
    public TextMeshProUGUI avaliableSkillPoints;
    public HealthController healthController; // Reference to the HealthController script
    public EnemyHealthController enemyHealthController; // Reference to the EnemyHealthController script
    public PlayerController playerController; // Reference to the PlayerController script
    public AmmoManager ammoManager; // Reference to the AmmoManager script
    public SoundController soundController; // Reference to the SoundController script
    public TextMeshProUGUI insufficientSkillPointsText; 
    private int level = 1;
    private int xpToNextLevel = 10;
    private static int skillPoints = 50; // Skill points that the player can use
    public List<Button> upgradeButtons; // List of upgrade buttons
    public List<TextMeshProUGUI> upgradeCostTexts; // List of upgrade cost texts
    private float xpMultiplier = 1.0f;

    private List<Upgrade> upgrades = new List<Upgrade>(); // List of available upgrades
    private Dictionary<System.Type, List<Upgrade>> upgradesByType = new Dictionary<System.Type, List<Upgrade>>(); // Group upgrades by type


    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        enemyHealthController = FindObjectOfType<EnemyHealthController>();
        if (enemyHealthController == null)
        {
            Debug.LogError("EnemyHealthController not found in the scene.");
        }
    }

    private void Start()
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

        if (healthController != null && playerController != null)
        {
            upgrades.Add(new HealthUpgrade("Increase Health I", 1, healthController, 10));
            upgrades.Add(new HealthUpgrade("Increase Health II", 2, healthController, 15));
            upgrades.Add(new HealthUpgrade("Increase Health III", 3, healthController, 25));

            upgrades.Add(new DurabilityUpgrade("Increase Durability I", 1, healthController, 10));
            upgrades.Add(new DurabilityUpgrade("Increase Durability II", 2, healthController, 15));
            upgrades.Add(new DurabilityUpgrade("Increase Durability III", 3, healthController, 25));

            upgrades.Add(new XPGainUpgrade("Increase XP I", 1, this, 1.15f));
            upgrades.Add(new XPGainUpgrade("Increase XP II", 2, this, 1.3f));
            upgrades.Add(new XPGainUpgrade("Increase XP III", 3, this, 1.5f));

            upgrades.Add(new MedkitUpgrade("Increase Medkit Effectiveness I", 1, healthController, 1.1f));
            upgrades.Add(new MedkitUpgrade("Increase Medkit Effectiveness II", 2, healthController, 1.25f));
            upgrades.Add(new MedkitUpgrade("Increase Medkit Effectiveness III", 3, healthController, 1.5f));

            upgrades.Add(new CritChanceUpgrade("Increase Crit Chance I", 1, 10));
            upgrades.Add(new CritChanceUpgrade("Increase Crit Chance II", 1, 17));
            upgrades.Add(new CritChanceUpgrade("Increase Crit Chance III", 1, 27));

            upgrades.Add(new CritDamageUpgrade("Increase Crit Damage I", 1, 175));
            upgrades.Add(new CritDamageUpgrade("Increase Crit Damage II", 1, 200));
            upgrades.Add(new CritDamageUpgrade("Increase Crit Damage III", 1, 225));

            upgrades.Add(new DropChanceUpgrade("Increase Drop Chance I", 1, 100f));
            upgrades.Add(new DropChanceUpgrade("Increase Drop Chance II", 2, 100f));
            upgrades.Add(new DropChanceUpgrade("Increase Drop Chance III", 3, 80f));

            
            upgrades.Add(new AmmoEffectivenessUpgrade("Increase Ammo Effectiveness I", 1, ammoManager, 1.1f));
            upgrades.Add(new AmmoEffectivenessUpgrade("Increase Ammo Effectiveness II", 2, ammoManager, 1.25f));
            upgrades.Add(new AmmoEffectivenessUpgrade("Increase Ammo Effectiveness III", 3, ammoManager, 1.5f));
        }
        else
        {
            Debug.LogError("HealthController or PlayerController is not assigned in XPManager.");
        }

        // Group upgrades by type, name was getting some issues before so changed to GetType()
        upgradesByType = upgrades.GroupBy(u => u.GetType()).ToDictionary(g => g.Key, g => g.OrderBy(u => u.skillPointsCost).ToList());
        UpdateSkillPointsText();
        UpdateUpgradeButtons();
    }

    public void SetXPMultiplier(float multiplier)
    {
        xpMultiplier = multiplier;
    }

    public void AddXP(int amount)
    {
        int adjustedXP = Mathf.RoundToInt(amount * xpMultiplier);
        xp += adjustedXP;
        Debug.Log("XP added: " + adjustedXP + " (original: " + amount + "). Total XP: " + xp);
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
            xpToNextLevel += level*10; // Increase the XP required for the next level
            level++;
            soundController.Play(soundController.levelUp, 0.3f);
            ShowLevelUpText();
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
        Debug.Log("Skill points: " + skillPoints);
        if (skillPointsText != null)
        {
            skillPointsText.text = "Skill Points: " + skillPoints;
        }
    }

    private void ShowLevelUpText()
    {
        if (levelUpText != null)
        {
            levelUpText.text = "Level Up!";
            avaliableSkillPoints.text = "Available Skill Points: " + skillPoints;
            levelUpText.gameObject.SetActive(true);
            StartCoroutine(HideLevelUpText());
        }
    }

    private IEnumerator HideLevelUpText()
    {
        yield return new WaitForSeconds(5f);
        levelUpText.gameObject.SetActive(false);
        avaliableSkillPoints.text = "";
    }

    // Update the upgrade buttons with the available upgrades
    private void UpdateUpgradeButtons()
    {
        Debug.Log("Updating upgrade buttons");
        int buttonIndex = 0;

        // Check if upgradesByType is populated
        if (upgradesByType == null || upgradesByType.Count == 0)
        {
            Debug.LogError("upgradesByType is null or empty");
            return;
        }

        // Check if upgradeButtons and upgradeCostTexts are correctly assigned
        if (upgradeButtons == null || upgradeCostTexts == null)
        {
            Debug.LogError("upgradeButtons or upgradeCostTexts is null");
            return;
        }

        // Loop through the upgrades by type
        foreach (var upgradeGroup in upgradesByType)
        {
            // Check if the button index is within the upgrade buttons and upgrade cost texts
            if (buttonIndex < upgradeButtons.Count && buttonIndex < upgradeCostTexts.Count)
            {
                // Get the first upgrade that is not purchased
                Upgrade currentUpgrade = upgradeGroup.Value.FirstOrDefault(u => !u.IsPurchased);
                // If there is an upgrade available that is not purchased yet set the button text and cost
                if (currentUpgrade != null)
                {
                    Debug.Log($"Setting button {buttonIndex} text to {currentUpgrade.name} and cost to {currentUpgrade.skillPointsCost}");
                    upgradeButtons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text = currentUpgrade.name;
                    upgradeCostTexts[buttonIndex].text = "Cost: " + currentUpgrade.skillPointsCost;
                    int index = buttonIndex; // Capture the index for the lambda
                    upgradeButtons[buttonIndex].onClick.RemoveAllListeners();
                    upgradeButtons[buttonIndex].onClick.AddListener(() => UseSkillPointsForUpgrade(currentUpgrade.GetType()));
                }
                else
                {
                    // If no upgrade is available, disable the button
                    Debug.Log($"No available upgrade for button {buttonIndex}, disabling button");
                    upgradeButtons[buttonIndex].interactable = false;
                }
            }
            else
            {
                Debug.LogError($"Button index {buttonIndex} is out of range for upgradeButtons or upgradeCostTexts");
            }
            buttonIndex++;
        }
    }


    private void SetButtonToMaxLevel(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.gray;
        colors.highlightedColor = Color.gray;
        colors.pressedColor = Color.gray;
        colors.selectedColor = Color.gray;
        button.colors = colors;
        button.interactable = false;
    }

    public void UseSkillPointsForUpgrade(System.Type upgradeType)
    {
        if (upgradesByType.ContainsKey(upgradeType))
        {
            Upgrade upgrade = upgradesByType[upgradeType].FirstOrDefault(u => !u.IsPurchased);
            if (upgrade != null)
            {
                if (UseSkillPoints(upgrade.skillPointsCost))
                {
                    upgrade.Purchase();
                    upgrade.ApplyUpgrade();
                    soundController.Play(soundController.skillPointPurchase, 0.3f);
                    Debug.Log("Skill points used for " + upgrade.name);
                    UpdateUpgradeButtons(); // Update the button text and cost
                    insufficientSkillPointsText.text = ""; // Clear the message
                }
                else
                {
                    Debug.Log("Not enough skill points for " + upgrade.name);
                    insufficientSkillPointsText.text = "Not enough skill points for " + upgrade.name;
                }
            }
        }
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
}
