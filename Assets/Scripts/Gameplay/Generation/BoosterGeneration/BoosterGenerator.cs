using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoosterGenerator : MonoBehaviour, IInitializable
{
    [SerializeField] private ObjectVisibilityWatcher objectVisibilityWatcher;
    [SerializeField] private BoosterPool boosterPool;

    [Inject] private RoadGenerator roadGenerator;
    [Inject] private ObstacleGenerator obstacleGenerator;
    [Inject] private BoosterGenerationSettings boosterGenerationSettings;

    List<BaseBoost> boosters = new();

    public void Initialize()
    {
        objectVisibilityWatcher.Init();
        objectVisibilityWatcher.OnObjectNonVisible += ObjectVisibilityWatcher_OnObjectNonVisible;
        obstacleGenerator.OnObstaclesCreated += ObstacleGenerator_OnObstaclesCreated;
    }

    private void OnDestroy()
    {
        objectVisibilityWatcher.OnObjectNonVisible -= ObjectVisibilityWatcher_OnObjectNonVisible;
        obstacleGenerator.OnObstaclesCreated += ObstacleGenerator_OnObstaclesCreated;
    }

    private void ObstacleGenerator_OnObstaclesCreated(RoadChunk roadChunk, List<ObstacleGenerator.ObstacleType> obstacles)
    {
        TryToCreateObstacle(roadChunk, obstacles);
    }

    private void TryToCreateObstacle(RoadChunk roadChunk, List<ObstacleGenerator.ObstacleType> obstacles)
    {
        bool shouldSpawn = boosterGenerationSettings.spawnProbability > 1f.Random();
        if (!shouldSpawn) return;

        int laneIndex = obstacles.FindIndex((x) => x == ObstacleGenerator.ObstacleType.none);
        if (laneIndex == -1) return;

        float x = roadGenerator.LaneXCoord(laneIndex);
        float z = roadChunk.transform.position.z;

        CreateBooster(new Vector3(x, 0f, z));
    }

    private BaseBoost CreateBooster(Vector3 position)
    {
        var booster = boosterPool.GetRandomObject();
        booster.transform.position = position;
        boosters.Add(booster);
        objectVisibilityWatcher.AddObject(booster.transform);
        return booster;
    }

    private void RemoveBooster(BaseBoost booster)
    {
        booster.Hide(true);
        boosters.Remove(booster);
        objectVisibilityWatcher.RemoveObject(booster.transform);
    }

    private void ObjectVisibilityWatcher_OnObjectNonVisible(Transform tr)
    {
        BaseBoost boosterToHide = boosters.Find((x) => x.transform == tr);
        RemoveBooster(boosterToHide);
    }

    public void ResetBoosters()
    {
        List<BaseBoost> boostersToRemove = new(boosters);
        boostersToRemove.ForEach((booster) => RemoveBooster(booster));
    }
}
