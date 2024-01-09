using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterFlySettings
{
    public float heigth = 3f;
    public float takeOfDuration = 0.4f;
    public AnimationCurve takeOfCurve;
    public AnimationCurve landingCurve;
}
