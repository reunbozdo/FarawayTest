using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class RoadGenerator : MonoBehaviour, IInitializable
{
    public event Action<RoadChunk> OnRoadChunkCreated;

    [SerializeField] private ObjectVisibilityWatcher objectVisibilityWatcher;
    [SerializeField] private RoadChunkPool roadChunkPool;

    [Inject] private RoadGenerationSettings roadGenerationSettings;

    private List<float> laneXCoords = new();
    private List<RoadChunk> roadChunks = new();

    public List<float> LanePositions => laneXCoords;

    public float LaneXCoord(int index) => laneXCoords[index];
    public float MiddleOfTheRoad => (laneXCoords.LastObject() + laneXCoords.First()) / 2f;    

    public void Initialize()
    {
        objectVisibilityWatcher.Init();
        objectVisibilityWatcher.OnObjectNonVisible += ObjectVisibilityWatcher_OnObjectNonVisible;
    }

    public void GenerateRoad()
    {   
        CalculateXCoordForLanes();
        GenerateAllChunks();
    }

    private void OnDestroy()
    {
        objectVisibilityWatcher.OnObjectNonVisible -= ObjectVisibilityWatcher_OnObjectNonVisible;
    }

    private void CalculateXCoordForLanes()
    {
        for (int i = 0; i < roadGenerationSettings.laneCount; i++)
        {
            float x = i * roadGenerationSettings.distanceBetweenLanes;
            laneXCoords.Add(x);
        }
    }

    private void GenerateAllChunks()
    {
        float zPos = 0f;
        for (int i = 0; i < roadGenerationSettings.roadChunkCount; i++)
        {
            CreateRoadChunk(zPos);
            zPos += roadGenerationSettings.lanePrefab.Length;
        }
    }

    private void CreateRoadChunk(float zPos)
    {
        RoadChunk roadChunk = roadChunkPool.GetObject();
        roadChunk.transform.position = new Vector3(0f, 0f, zPos);

        roadChunk.Init(laneXCoords, roadGenerationSettings.lanePrefab);

        roadChunks.Add(roadChunk);
        objectVisibilityWatcher.AddObject(roadChunk.transform);

        OnRoadChunkCreated?.Invoke(roadChunk);
    }

    private void ObjectVisibilityWatcher_OnObjectNonVisible(Transform tr)
    {
        RoadChunk roadChunkToHide = roadChunks.Find((x) => x.transform == tr);
        RemoveRoadChunk(roadChunkToHide);

        float zPos = roadChunks.Last().transform.position.z + roadGenerationSettings.lanePrefab.Length;
        CreateRoadChunk(zPos);
    }

    private void RemoveRoadChunk(RoadChunk roadChunk)
    { 
        roadChunk.Hide(true);
        roadChunks.Remove(roadChunk);
        objectVisibilityWatcher.RemoveObject(roadChunk.transform);
    }

    public void ResetRoad()
    {
        List<RoadChunk> roadChunksToRemove = new(roadChunks);
        roadChunksToRemove.ForEach((roadChunk) => RemoveRoadChunk(roadChunk));

        GenerateAllChunks();
    }

    public void LoseAnimation()
    {
        foreach (RoadChunk roadChunk in roadChunks)
        {
            roadChunk.Hide();
        }
    }
}
