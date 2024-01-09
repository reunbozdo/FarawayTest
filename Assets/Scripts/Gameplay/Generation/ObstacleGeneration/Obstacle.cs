using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : VisibilityAnimationController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if (player != null) player.ObstacleHit();
        }
    }
}
