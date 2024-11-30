using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public static int xp = 0;
    public Slider xpSlider; // Reference to the XP Slider

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
            xpSlider.maxValue = 100; // Set this to the maximum XP value for the current level
            xpSlider.value = xp;
        }
    }

    public void AddXP(int amount)
    {
        xp += amount;
        Debug.Log("XP added: " + amount + ". Total XP: " + xp);
        UpdateXPSlider();
    }

    public int GetXP()
    {
        return xp;
    }

    private void UpdateXPSlider()
    {
        if (xpSlider != null)
        {
            xpSlider.value = xp;
        }
    }
}
