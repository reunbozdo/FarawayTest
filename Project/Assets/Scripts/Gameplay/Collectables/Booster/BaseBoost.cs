using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BaseBoost : VisibilityAnimationController, ICollectable
{
    [SerializeField] private float effectLenghtSec = 7f;

    [Inject] protected Player player;

    private bool collected;

    public void Collect()
    {
        if (collected) return;

        collected = true;

        StartEffect();

        Type boostType = GetType();
        Action stopEffect = () =>
        {
            StopEffect();
            player.PlayerActiveBoostController.RemoveActiveBoost(boostType);            
        };
        int stopTaskID = stopEffect.Timer(effectLenghtSec);

        player.PlayerActiveBoostController.AddActiveBoost(boostType, stopTaskID);

        Hide();
    }

    protected abstract void StartEffect();

    protected abstract void StopEffect();
}
