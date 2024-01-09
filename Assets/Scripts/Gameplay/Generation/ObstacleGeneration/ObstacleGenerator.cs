using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleGenerator : MonoBehaviour, IInitializable
{
    public enum ObstacleType
    {
        none = 0,
        avoidable = 1,
        unavoidable = 2
    }

    public event Action<RoadChunk, List<ObstacleType>> OnObstaclesCreated;

    [SerializeField] private ObjectVisibilityWatcher objectVisibilityWatcher;
    [SerializeField] private AvoidableObstaclePool avoidableObstaclePool;
    [SerializeField] private UnavoidableObstaclePool unavoidableObstaclePool;

    [Inject] private RoadGenerator roadGenerator;
    [Inject] private ObstacleGenerationSettings obstacleGenerationSettings;

    private List<Obstacle> obstacles = new();

    public void Initialize()
    {
        objectVisibilityWatcher.Init();
        objectVisibilityWatcher.OnObjectNonVisible += ObjectVisibilityWatcher_OnObjectNonVisible;
        roadGenerator.OnRoadChunkCreated += RoadGenerator_OnRoadChunkCreated;
    }

    private void OnDestroy()
    {
        roadGenerator.OnRoadChunkCreated -= RoadGenerator_OnRoadChunkCreated;
        objectVisibilityWatcher.OnObjectNonVisible -= ObjectVisibilityWatcher_OnObjectNonVisible;
    }

    private void RoadGenerator_OnRoadChunkCreated(RoadChunk roadChunk)
    {
        FillWithObstacles(roadChunk);
    }

    private void FillWithObstacles(RoadChunk roadChunk)
    {
        float z = roadChunk.transform.position.z;

        float obstacleSpawnProbability = obstacleGenerationSettings.GetObstacleSpawnProbability(z);

        List<ObstacleType> obstaclesToSpawn = GetObstaclesToSpawn(obstacleSpawnProbability);        

        for (int i = 0; i < obstaclesToSpawn.Count; i++)
        {
            float x = roadGenerator.LanePositions[i];
            ObstacleType obstacleType = obstaclesToSpawn[i];

            Vector3 obstaclePosition = new Vector3(x, 0f, z);

            CreateObstacle(obstacleType, obstaclePosition);
        }

        OnObstaclesCreated?.Invoke(roadChunk, obstaclesToSpawn);
    }

    private List<ObstacleType> GetObstaclesToSpawn(float obstacleSpawnProbability)
    {
        List<ObstacleType> obstaclesToSpawn = new();
        foreach (float x in roadGenerator.LanePositions)
        {
            if (obstacleSpawnProbability > 1f.Random())
            {
                obstaclesToSpawn.Add(0.5f > 1f.Random() ? ObstacleType.avoidable : ObstacleType.unavoidable);
            }
            else
            {
                obstaclesToSpawn.Add(ObstacleType.none);
            }
        }

        if (!obstaclesToSpawn.Contains(ObstacleType.avoidable) && !obstaclesToSpawn.Contains(ObstacleType.none))
        {
            int indexOfObstacleToReplace = obstaclesToSpawn.FindIndex((x) => x == ObstacleType.unavoidable);
            obstaclesToSpawn[indexOfObstacleToReplace] = 0.5f > 1f.Random() ? ObstacleType.none : ObstacleType.avoidable;
        }

        return obstaclesToSpawn;
    }

    private void ObjectVisibilityWatcher_OnObjectNonVisible(Transform tr)
    {
        Obstacle obstacleToHide = obstacles.Find((x) => x.transform == tr);
        RemoveObstacle(obstacleToHide);
    }

    private Obstacle CreateObstacle(ObstacleType obstacleType, Vector3 position)
    {
        if (obstacleType == ObstacleType.none) return null;

        Obstacle obstacle = obstacleType == ObstacleType.unavoidable ? unavoidableObstaclePool.GetRandomObject(false) : avoidableObstaclePool.GetRandomObject(false);
        obstacle.transform.position = position;
        obstacles.Add(obstacle);
        objectVisibilityWatcher.AddObject(obstacle.transform);
        return obstacle;
    }

    private void RemoveObstacle(Obstacle obstacle)
    {
        obstacle.Hide(true);
        obstacles.Remove(obstacle);
        objectVisibilityWatcher.RemoveObject(obstacle.transform);
    }

    public void ResetObstacles()
    {
        List<Obstacle> obstaclesToRemove = new(obstacles);
        obstaclesToRemove.ForEach((obstacle) => RemoveObstacle(obstacle));
    }

    public void LoseAnimation()
    {
        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.Hide();
        }
    }
}
