using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class PlayerSpeedController : MonoBehaviour
{
    private float baseSpeed;
    public float BaseSpeed => baseSpeed;

    public float CurrentSpeed { get; private set; }
    public float TargetSpeed { get; private set; }

    public float NormalizedSpeed => CurrentSpeed / baseSpeed;

    public void Init(float baseSpeed)
    {
        this.baseSpeed = baseSpeed;
    }
    
    public void ChangeSpeed(float endSpeed, float duration)
    {
        TargetSpeed = endSpeed;
        DOTween.To(() => CurrentSpeed, x => CurrentSpeed = x, endSpeed, duration);
    }
}
