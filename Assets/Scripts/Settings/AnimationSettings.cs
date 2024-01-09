using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "Settings/AnimationSettings")]
public class AnimationSettings : ScriptableObject
{
    public enum AnimationType
    {
        none = 0,
        scale = 1,
        fade = 2
    }

    [EnumToggleButtons]
    public AnimationType animation = AnimationType.scale;

    [HideIf("animation", AnimationType.none)]
    public float duration = 0.2f;


    [ShowIf("animation", AnimationType.scale)]
    public AnimationCurve showCurve;

    [ShowIf("animation", AnimationType.scale)]
    public AnimationCurve hideCurve;
}
