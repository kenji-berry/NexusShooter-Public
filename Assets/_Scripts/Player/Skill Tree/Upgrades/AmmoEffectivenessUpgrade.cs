public class AmmoEffectivenessUpgrade : Upgrade
{
    private AmmoManager ammoManager;
    private float effectivenessMultiplier;

    public AmmoEffectivenessUpgrade(string name, int skillPointsCost, AmmoManager ammoManager, float effectivenessMultiplier)
        : base(name, skillPointsCost)
    {
        this.ammoManager = ammoManager;
        this.effectivenessMultiplier = effectivenessMultiplier;
    }

    public override void ApplyUpgrade()
    {
        ammoManager.SetAmmoEffectivenessMultiplier(effectivenessMultiplier);
    }
}