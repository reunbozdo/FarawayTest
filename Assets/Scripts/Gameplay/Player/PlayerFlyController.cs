using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class PlayerFlyController : MonoBehaviour
{
    public enum State
    {
        none = 0,
        takeOf = 1,
        flying = 2,
        landing = 3
    }

    [Inject] private CharacterFlySettings characterFlySettings;

    private PlayerAnimationController playerAnimationController;

    private string flyID => $"fly-{GetInstanceID()}";

    private State state = State.none;
    public State CurrentState => state;

    public void Init(PlayerAnimationController playerAnimationController)
    {
        this.playerAnimationController = playerAnimationController;
    }

    public void TakeOf()
    {
        playerAnimationController.Fly(characterFlySettings.takeOfDuration);

        state = State.takeOf;

        transform.DOLocalMoveY(characterFlySettings.heigth, characterFlySettings.takeOfDuration)
            .SetEase(characterFlySettings.takeOfCurve)
            .SetRelative()
            .OnComplete(() => state = State.flying)
            .SetId(flyID);
    }

    public void Landing()
    {
        playerAnimationController.StartRun(characterFlySettings.takeOfDuration);

        state = State.landing;

        transform.DOLocalMoveY(0f, characterFlySettings.takeOfDuration)
            .SetEase(characterFlySettings.landingCurve)
            .OnComplete(() => state = State.none)
            .SetId(flyID);
    }
}
