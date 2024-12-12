using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    private PlayerController playerController;
    private float speedIncrease;

    public SpeedUpgrade(string name, int skillPointsCost, PlayerController playerController, float speedIncrease)
        : base(name, skillPointsCost)
    {
        this.playerController = playerController;
        this.speedIncrease = speedIncrease;
    }

    public override void ApplyUpgrade()
    {
        playerController.IncreaseSpeed(speedIncrease);
    }
}