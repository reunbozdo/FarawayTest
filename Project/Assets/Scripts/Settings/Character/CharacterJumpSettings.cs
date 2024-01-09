using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterJumpSettings
{
    [Header("Jump")]
    public float jumpHeight = 1f;
    public float jumpDuration = 0.4f;
    public float jumpAnimationBlendDuration = 0.1f;
    public AnimationCurve jumpCurve;

    [Header("Break jump")]
    public float breakDuration;
    public AnimationCurve breakCurve;
}
