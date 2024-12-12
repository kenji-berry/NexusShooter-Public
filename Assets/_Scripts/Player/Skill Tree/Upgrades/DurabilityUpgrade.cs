public class DurabilityUpgrade : Upgrade
{
    private HealthController healthController;
    private int durabilityIncrease;

    public DurabilityUpgrade(string name, int skillPointsCost, HealthController healthController, int durabilityIncrease)
        : base(name, skillPointsCost)
    {
        this.healthController = healthController;
        this.durabilityIncrease = durabilityIncrease;
    }

    public override void ApplyUpgrade()
    {
        healthController.IncreaseArmourDurability(durabilityIncrease);
    }
}