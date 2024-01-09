using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterLaneSwitchSettings
{
    public int playerStartLane = 1;

    [Header("Lane switch animation")]
    public float duration = 0.4f;
    public AnimationCurve switchCurve;
}
