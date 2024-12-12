public class CritDamageUpgrade : Upgrade
{
    private EnemyHealthController enemyHealthController;
    private int newCritDamage;

    public CritDamageUpgrade(string name, int skillPointsCost, EnemyHealthController enemyHealthController, int newCritDamage)
        : base(name, skillPointsCost)
    {
        this.enemyHealthController = enemyHealthController;
        this.newCritDamage = newCritDamage;
    }

    public override void ApplyUpgrade()
    {
        enemyHealthController.increaseCritDamage(newCritDamage);
    }
}