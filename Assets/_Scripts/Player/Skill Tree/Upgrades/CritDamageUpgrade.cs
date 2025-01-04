using UnityEngine;

public class CritDamageUpgrade : Upgrade
{
    private int newCritDamage;

    public CritDamageUpgrade(string name, int skillPointsCost, int newCritDamage)
        : base(name, skillPointsCost)
    {
        this.newCritDamage = newCritDamage;
    }

    public override void ApplyUpgrade()
    {
        EnemyHealthController[] enemyHealthControllers = GameObject.FindObjectsByType<EnemyHealthController>(FindObjectsSortMode.None);
        foreach (var enemyHealthController in enemyHealthControllers)
        {
            enemyHealthController.increaseCritDamage(newCritDamage);
        }
    }
}