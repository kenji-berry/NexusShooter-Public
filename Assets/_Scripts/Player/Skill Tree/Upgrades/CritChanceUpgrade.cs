using UnityEngine;

public class CritChanceUpgrade : Upgrade
{
    private int newCritChance;

    public CritChanceUpgrade(string name, int skillPointsCost, int newCritChance)
        : base(name, skillPointsCost)
    {
        this.newCritChance = newCritChance;
    }

    public override void ApplyUpgrade()
    {
        EnemyHealthController[] enemyHealthControllers = GameObject.FindObjectsByType<EnemyHealthController>(FindObjectsSortMode.None);
        foreach (var enemyHealthController in enemyHealthControllers)
        {
            enemyHealthController.increaseCritChance(newCritChance);
        }
    }
}