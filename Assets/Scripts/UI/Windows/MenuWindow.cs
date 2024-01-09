using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MenuWindow : BaseWindow
{
    [SerializeField] private BaseButton runButton;
    [SerializeField] private BaseText highscoreText;

    [Inject] private GameManager gameManager;
    [Inject] private Statistics statistics;

    public override void Init()
    {
        base.Init();
        runButton.SetOnClick(OnRunButtonClick);
    }

    protected override void BeforeShow()
    {
        highscoreText.SetNumber(statistics.Highscore, true);
    }

    private void OnRunButtonClick()
    {
        gameManager.StartRun();       
    }
}
