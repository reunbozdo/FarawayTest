using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IInitializable
{
    public event Action OnStartRun;
    public event Action OnObstacleHit;

    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] private PlayerSpeedController playerSpeedController;
    [SerializeField] private PlayerMoveSystem playerMoveSystem;
    [SerializeField] private PlayerLaneController playerLaneController;
    [SerializeField] private PlayerJumpController playerJumpController;
    [SerializeField] private PlayerCrawlController playerCrawlController;
    [SerializeField] private PlayerActiveBoostController playerActiveBoostController;
    [SerializeField] private PlayerFlyController playerFlyController;
    [SerializeField] private PlayerScoreController playerScoreController;
    [SerializeField] private Ragdoll ragdoll;

    [Inject] private CharacterSpeedSettings characterSpeedSettings;
    [Inject] private GameManager gameManager;

    public PlayerSpeedController PlayerSpeedController => playerSpeedController;
    public PlayerActiveBoostController PlayerActiveBoostController => playerActiveBoostController;
    public PlayerFlyController PlayerFlyController => playerFlyController;
    public PlayerScoreController PlayerScoreController => playerScoreController;

    public void Initialize()
    {
        playerSpeedController.Init(characterSpeedSettings.baseSpeed);
        playerAnimationController.Init(playerSpeedController);
        playerMoveSystem.Init(playerSpeedController);
        playerJumpController.Init(playerAnimationController, playerCrawlController, playerFlyController);
        playerCrawlController.Init(playerAnimationController, playerJumpController, playerFlyController);
        playerFlyController.Init(playerAnimationController);
        playerScoreController.Init();

        playerAnimationController.Idle(0f);

        DisablePlayerMovement();
    }

    public void StartRun()
    {
        ActivatePlayerMovement();
        playerSpeedController.ChangeSpeed(characterSpeedSettings.baseSpeed, characterSpeedSettings.speedChangeDuration);
        playerAnimationController.StartRun(characterSpeedSettings.speedChangeDuration);        

        OnStartRun?.Invoke();
    }

    public void ResetPlayer()
    {
        DisablePlayerMovement();
        ragdoll.transform.localEulerAngles = Vector3.zero;
        ragdoll.transform.localPosition = Vector3.zero;
        playerSpeedController.ChangeSpeed(0f, 0f);
        playerAnimationController.Idle(0f);
        playerLaneController.PutPlayerAtStart();
        ragdoll.DisableRagdoll();
    }

    public void ObstacleHit()
    {
        DisablePlayerMovement();
        ragdoll.EnableRagdoll();
        gameManager.LoseRun();
        playerAnimationController.StopCurrentAnimation();

        OnObstacleHit?.Invoke();
    }

    public void ActivatePlayerMovement()
    {
        playerMoveSystem.StartRun();
        playerLaneController.Activate();
        playerJumpController.Activate();
        playerCrawlController.Activate();
    }

    public void DisablePlayerMovement()
    {
        playerMoveSystem.Stop();
        playerLaneController.Disable();
        playerJumpController.Disable();
        playerCrawlController.Disable();
    }

    public void OnTriggerEnter(Collider other)
    {        
        if (other.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Collect();
        }
    }
}

