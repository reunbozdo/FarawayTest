using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChunk : VisibilityAnimationController
{
    private List<Lane> createdLanes = new List<Lane>();

    private bool inited = false;

    public void Init(List<float> laneXCoords, Lane lanePrefab)
    {
        if (inited) return;
        foreach (float xPos in laneXCoords)
        {
            Lane lane = Instantiate(lanePrefab, transform);
            lane.transform.localPosition = new Vector3(xPos, 0f, 0f);

            createdLanes.Add(lane);
        }
        inited = true;
    }
}
