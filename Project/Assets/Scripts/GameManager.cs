using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour, IInitializable
{
    [SerializeField] private Player player;

    [Inject] private WindowController windowController;
    [Inject] private RoadGenerator roadGenerator;
    [Inject] private ObstacleGenerator obstacleGenerator;
    [Inject] private FadeTransition fadeTransition;

    public void Initialize()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {        
        StartGame();
    }

    [Button]
    public void StartGame()
    {
        roadGenerator.GenerateRoad();
        player.ResetPlayer();
        windowController.OpenWindow<MenuWindow>();        
    }

    [Button]
    public void StartRun()
    {
        windowController.OpenWindow<GameWindow>(callback: player.StartRun);
    }

    [Button]
    public void LoseRun()
    {
        windowController.OpenWindow<LoseWindow>();        
    }

    [Button]
    public void RestartGame()
    {
        windowController.OpenWindow<MenuWindow>();

        Action gameRestart = () =>
        {
            obstacleGenerator.ResetObstacles();
            roadGenerator.ResetRoad();
            player.ResetPlayer();
            
        };
        
        fadeTransition.Transition(gameRestart);
    }
}
