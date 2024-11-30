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

    private int level = 1;
    private int xpToNextLevel = 10;

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
            UpdateLevelText();
        }
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
}
