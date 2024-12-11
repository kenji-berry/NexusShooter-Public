public class CritChanceUpgrade : Upgrade
{
    private EnemyHealthController enemyHealthController;
    private int critChanceIncrease;

    public CritChanceUpgrade(string name, int skillPointsCost, EnemyHealthController enemyHealthController, int critChanceIncrease)
        : base(name, skillPointsCost)
    {
        this.enemyHealthController = enemyHealthController;
        this.critChanceIncrease = critChanceIncrease;
    }

    public override void ApplyUpgrade()
    {
        enemyHealthController.increaseCritChance(critChanceIncrease);
    }
}