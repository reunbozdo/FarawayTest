using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonBehaviourSettings", menuName = "Settings/Button Behaviour Settings")]
public class ButtonBehaviourSettings : ScriptableObject
{
    public bool PressedState = true;
    [ShowIf("PressedState")]
    public float PressedStateDuration = 0.1f;
    [ShowIf("PressedState")]
    public float PressedScaleMultiplier = 0.85f;
    [ShowIf("PressedState")]
    public AnimationCurve PressStateDownCurve;
    [ShowIf("PressedState")]
    public AnimationCurve PressStateUpCurve;
}
