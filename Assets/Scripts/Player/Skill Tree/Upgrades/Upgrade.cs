public abstract class Upgrade
{
    public string name;
    public int skillPointsCost;

    public Upgrade(string name, int skillPointsCost)
    {
        this.name = name;
        this.skillPointsCost = skillPointsCost;
    }

    public abstract void ApplyUpgrade();
}