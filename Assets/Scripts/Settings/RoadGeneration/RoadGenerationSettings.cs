using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadGenerationSettings
{
    public RoadChunk roadChunkPrefab;
    public Lane lanePrefab;

    public int laneCount = 3;

    public float distanceBetweenLanes = 2f;

    public int roadChunkCount = 10;
}
