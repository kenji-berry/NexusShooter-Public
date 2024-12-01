public class MedkitUpgrade : Upgrade
{
    private HealthController healthController;
    private float effectivenessMultiplier;

    public MedkitUpgrade(string name, int skillPointsCost, HealthController healthController, float effectivenessMultiplier)
        : base(name, skillPointsCost)
    {
        this.healthController = healthController;
        this.effectivenessMultiplier = effectivenessMultiplier;
    }

    public override void ApplyUpgrade()
    {
        healthController.SetMedkitEffectivenessMultiplier(effectivenessMultiplier);
    }
}