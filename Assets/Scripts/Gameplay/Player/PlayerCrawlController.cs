using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerCrawlController : MonoBehaviour
{
    public enum State
    {
        none = 0,
        performingCrawl = 1,
        breakingCrawl = 2
    }

    [Inject] private CharacterCrawlSettings characterCrawlSettings;

    private PlayerAnimationController playerAnimationController;
    private PlayerJumpController playerJumpController;
    private PlayerFlyController playerFlyController;

    private State state = State.none;
    public State CurrentState => state;

    public void Init(PlayerAnimationController playerAnimationController, PlayerJumpController playerJumpController, PlayerFlyController playerFlyController)
    {
        this.playerJumpController = playerJumpController;
        this.playerAnimationController = playerAnimationController;
        this.playerFlyController = playerFlyController;
    }

    private void OnDestroy()
    {
        Disable();
    }

    private void SwipeHandler_OnSwipe(SwipeHandler.Direction direction)
    {
        if (playerJumpController.CurrentState != PlayerJumpController.State.none) return;
        if (playerFlyController.CurrentState != PlayerFlyController.State.none) return;

        switch (direction)
        {
            case SwipeHandler.Direction.up:
                BreakCrawl();
                break;
            case SwipeHandler.Direction.down:
                Crawl();
                break;
        }
    }

    private void Crawl()
    {
        if (state != State.none) return;

        state = State.performingCrawl;
        playerAnimationController.Crawl(characterCrawlSettings.animationChangeDuration, characterCrawlSettings.crawlDuration, () => state = State.none);
    }

    private void BreakCrawl()
    {
        if (state != State.performingCrawl) return;

        state = State.breakingCrawl;
        playerAnimationController.StopCurrentAnimation();
        playerAnimationController.StartRun(characterCrawlSettings.animationChangeDuration);
        state = State.none;
    }

    public void Activate()
    {
        SwipeHandler.OnSwipe += SwipeHandler_OnSwipe;
    }

    public void Disable()
    {
        SwipeHandler.OnSwipe -= SwipeHandler_OnSwipe;
    }
}