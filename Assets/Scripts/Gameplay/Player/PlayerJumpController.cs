using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerJumpController : MonoBehaviour
{
    public enum State
    {
        none = 0,
        performingJump = 1,
        breakingJump = 2
    }

    [Inject] private CharacterJumpSettings characterJumpSettings;

    private PlayerAnimationController playerAnimationController;
    private PlayerCrawlController playerCrawlController;
    private PlayerFlyController playerFlyController;

    private string jumpID => $"jump-{GetInstanceID()}";

    private State state = State.none;
    public State CurrentState => state;

    public void Init(PlayerAnimationController playerAnimationController, PlayerCrawlController playerCrawlController, PlayerFlyController playerFlyController)
    {
        this.playerAnimationController = playerAnimationController;
        this.playerCrawlController = playerCrawlController;
        this.playerFlyController = playerFlyController;
    }

    private void OnDestroy()
    {
        Disable();
    }

    private void SwipeHandler_OnSwipe(SwipeHandler.Direction direction)
    {
        if (playerCrawlController.CurrentState != PlayerCrawlController.State.none) return;
        if (playerFlyController.CurrentState != PlayerFlyController.State.none) return;

        switch (direction)
        {
            case SwipeHandler.Direction.up:
                Jump();
                break;
            case SwipeHandler.Direction.down:
                BreakJump();
                break;
        }
    }

    private void BreakJump()
    {
        if (state != State.performingJump) return;

        state = State.breakingJump;
        DOTween.Kill(jumpID);
        playerAnimationController.StopCurrentAnimation();

        transform.DOMoveY(0f, characterJumpSettings.breakDuration)
            .SetEase(characterJumpSettings.breakCurve)
            .OnComplete(() => state = State.none);

        playerAnimationController.StartRun(characterJumpSettings.breakDuration);
    }

    private void Jump()
    {
        if (state != State.none) return;

        state = State.performingJump;

        transform.DOLocalMoveY(characterJumpSettings.jumpHeight, characterJumpSettings.jumpDuration)
            .SetEase(characterJumpSettings.jumpCurve)
            .SetRelative()
            .OnComplete(() => state = State.none)
            .SetId(jumpID);

        float blendDuration = characterJumpSettings.jumpAnimationBlendDuration;

        playerAnimationController.Jump(blendDuration, characterJumpSettings.jumpDuration, () => playerAnimationController.StartRun(blendDuration));
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