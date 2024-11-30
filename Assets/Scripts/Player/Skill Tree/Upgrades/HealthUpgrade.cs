public class HealthUpgrade : Upgrade
{
    private HealthController healthController;
    private int healthIncrease;

    public HealthUpgrade(string name, int skillPointsCost, HealthController healthController, int healthIncrease)
        : base(name, skillPointsCost)
    {
        this.healthController = healthController;
        this.healthIncrease = healthIncrease;
    }

    public override void ApplyUpgrade()
    {
        healthController.IncreaseMaxHealth(healthIncrease);
    }
}