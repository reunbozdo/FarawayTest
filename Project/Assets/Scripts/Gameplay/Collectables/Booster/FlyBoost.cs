using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBoost : BaseBoost
{   
    protected override void StartEffect()
    {
        player.PlayerFlyController.TakeOf();
    }

    protected override void StopEffect()
    {
        player.PlayerFlyController.Landing();
    }
}
