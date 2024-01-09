using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TextBehaviourSettings", menuName = "Settings/Text Behaviour Settings")]
public class TextBehaviourSettings : ScriptableObject
{
    [Header("Number count settings")]
    public float durationOfNumberCounting = 0.2f;

    public bool scaleOnUpdate;
    [ShowIf("scaleOnUpdate")]
    public ShakeSettings shakeSettings;
}
