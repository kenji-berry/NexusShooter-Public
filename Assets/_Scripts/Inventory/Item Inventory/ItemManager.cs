using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private HealthController healthController;
    private PlayerController playerController;

    void Awake()
    {
        healthController = gameObject.GetComponent<HealthController>();
        playerController = gameObject.GetComponent<PlayerController>();

        if (healthController == null)
        {
            Debug.LogError("Could not find health controller");
        }

        if (playerController == null)
        {
            Debug.LogError("Could not find player controller");
        }
    }

    public void UseItem(int id)
    {
        switch (id)
        {
            case 4: // Armour Repair Kit
                healthController.RepairArmour();
                break;

            case 6: // Stim Shot
                healthController.Heal(50);
                playerController.ActivateSpeedBoost(1.25f, 4);
                break;
        }
    }
}
