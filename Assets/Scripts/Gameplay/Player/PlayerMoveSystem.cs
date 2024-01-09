using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

public class PlayerMoveSystem : MonoBehaviour
{
    [SerializeField] private float smoothFactor = 0.5f;

    private PlayerSpeedController playerSpeedController;

    IDisposable moveSystem;

    public void Init(PlayerSpeedController playerSpeedController)
    {
        this.playerSpeedController = playerSpeedController;
    }

    public void StartRun()
    {
        moveSystem = Observable.EveryFixedUpdate()
            .Subscribe((_) => {
                HandleMove();
            }).AddTo(this);
    }

    public void Stop()
    {
        moveSystem?.Dispose();
    }

    private void HandleMove()
    {
        Vector3 newPos = transform.position += transform.forward * playerSpeedController.CurrentSpeed;
        transform.position = Vector3.Lerp(transform.position, newPos, smoothFactor * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        moveSystem?.Dispose();
    }
}
