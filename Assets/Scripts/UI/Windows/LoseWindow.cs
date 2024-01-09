using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoseWindow : BaseWindow
{
    [SerializeField] private BaseText scoreText, highscoreText;
    [SerializeField] private BaseButton restartButton;
    [SerializeField] private float pixelEffectShowDuration = 0.4f;
    [SerializeField] private float pixelEffectTargetWidth = 20;

    [Inject] private GameManager gameManager;
    [Inject] private Statistics statistics;
    [Inject] private FullscreenEffectController fullscreenEffectController;

    public override void Init()
    {
        base.Init();
        restartButton.SetOnClick(OnRestartClick);
    }

    protected override void BeforeShow()
    {
        fullscreenEffectController.TurnOnPixelEffect(pixelEffectShowDuration, pixelEffectTargetWidth);
        scoreText.SetNumber(statistics.CurrentScore);
        highscoreText.SetNumber(statistics.Highscore, true);
    }


    protected override void BeforeHide()
    {
        fullscreenEffectController.TurnOffPixelEffect(pixelEffectShowDuration);
    }

    private void OnRestartClick()
    {
        gameManager.RestartGame();
    }
}