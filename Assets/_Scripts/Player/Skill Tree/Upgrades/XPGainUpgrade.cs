public class XPGainUpgrade : Upgrade
{
    private float multiplier;
    private XPManager xpManager;

    public XPGainUpgrade(string name, int tier, XPManager xpManager, float multiplier) : base(name, tier)
    {
        this.multiplier = multiplier;
        this.xpManager = xpManager;
    }

    public float GetMultiplier()
    {
        return multiplier;
    }

    public override void ApplyUpgrade()
    {
        xpManager.SetXPMultiplier(multiplier);
    }
}