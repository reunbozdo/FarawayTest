using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Cinemachine;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    [SerializeField] private PlayerCameraPoint playerCameraPoint;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Player>().FromInstance(player).AsSingle();
        Container.Bind<PlayerCameraPoint>().FromInstance(playerCameraPoint).AsSingle();
    }
}
