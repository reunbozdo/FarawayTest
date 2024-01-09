using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : BaseBoost
{
    [SerializeField] private float speedMultiplier = 0.5f;
    [SerializeField] private float speedChangeDuration = 0.2f;

    protected override void StartEffect()
    {
        var speedController = player.PlayerSpeedController;
        float newSpeed = speedMultiplier * speedController.BaseSpeed;

        speedController.ChangeSpeed(newSpeed, speedChangeDuration);
    }

    protected override void StopEffect()
    {
        var speedController = player.PlayerSpeedController;
        float baseSpeed = speedController.BaseSpeed;

        speedController.ChangeSpeed(baseSpeed, speedChangeDuration);
    }
}
