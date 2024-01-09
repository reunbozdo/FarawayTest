using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShakeSettings", menuName = "Settings/Shake Settings")]
public class ShakeSettings : ScriptableObject
{
    public int vibrato = 4;
    public float duration = 0.2f;
    public Vector3 power = 10f.ToVector3();
}
