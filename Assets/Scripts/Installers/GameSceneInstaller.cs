using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RoadGenerator roadGenerator;
    [SerializeField] private ObstacleGenerator obstacleGenerator;
    [SerializeField] private BoosterGenerator boosterGenerator;
    [SerializeField] private FadeTransition fadeTransition;
    [SerializeField] private FullscreenEffectController fullscreenEffectController;

    public override void InstallBindings()
    {        
        Container.BindInterfacesAndSelfTo<WindowController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SwipeHandler>().AsSingle().NonLazy();
        Container.Bind<Statistics>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().FromInstance(gameManager).AsSingle();
        Container.BindInterfacesAndSelfTo<ObstacleGenerator>().FromInstance(obstacleGenerator).AsSingle();
        Container.BindInterfacesAndSelfTo<RoadGenerator>().FromInstance(roadGenerator).AsSingle();
        Container.BindInterfacesAndSelfTo<BoosterGenerator>().FromInstance(boosterGenerator).AsSingle();

        Container.Bind<FadeTransition>().FromInstance(fadeTransition).AsSingle();
        Container.Bind<FullscreenEffectController>().FromInstance(fullscreenEffectController).AsSingle();
    }
}
