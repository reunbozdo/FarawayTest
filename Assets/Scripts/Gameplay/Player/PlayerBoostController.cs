using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerActiveBoostController : MonoBehaviour
{
    [System.Serializable]
    public class ActiveBoostInfo
    {
        public Type type;
        public int endTaskID;

        public ActiveBoostInfo(Type type, int endTaskID)
        {
            this.type = type;
            this.endTaskID = endTaskID;
        }
    }

    [ShowInInspector]
    private List<ActiveBoostInfo> activeBoosts = new();

    public void AddActiveBoost(Type boostType, int taskID)
    {
        var sameBoost = activeBoosts.Find((x) => x.type == boostType);

        if (sameBoost != null)
        {
            activeBoosts.Remove(sameBoost);
            DOTween.Kill(sameBoost.endTaskID);
        }

        activeBoosts.Add(new ActiveBoostInfo(boostType, taskID));
    }

    public void RemoveActiveBoost(Type boostType)
    {
        activeBoosts.RemoveAll((x) => x.type == boostType);
    }
}
