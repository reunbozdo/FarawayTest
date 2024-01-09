using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleGenerationSettings
{
    public AnimationCurve obstacleSpawnProbabilityAtStart;
    public float startAreaLenght = 100f;

    public float GetObstacleSpawnProbability(float zPos)
    {
        float value = Mathf.Abs(zPos) / startAreaLenght;
        return obstacleSpawnProbabilityAtStart.Evaluate(value);
    }
}
