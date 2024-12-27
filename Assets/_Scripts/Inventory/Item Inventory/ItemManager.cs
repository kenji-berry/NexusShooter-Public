using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private HealthController healthController;

    void Awake()
    {
        healthController = gameObject.GetComponent<HealthController>();

        if (healthController == null)
        {
            Debug.LogError("Could not find health controller");
        }
    }

    public void UseItem(int id)
    {
        switch (id)
        {
            case 4: // Armour Repair Kit
                healthController.RepairArmour();
                break;
        }
    }
}
