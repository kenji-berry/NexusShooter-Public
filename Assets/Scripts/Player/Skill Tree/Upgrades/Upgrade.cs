public abstract class Upgrade
{
    public string name;
    public int skillPointsCost;
    public bool IsPurchased { get; private set; }

    public Upgrade(string name, int skillPointsCost)
    {
        this.name = name;
        this.skillPointsCost = skillPointsCost;
        this.IsPurchased = false; // Initialize as not purchased
    }

    public void Purchase()
    {
        IsPurchased = true;
    }

    public abstract void ApplyUpgrade();
}