using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameWindow : BaseWindow
{
    [SerializeField] private BaseText scoreCounter;

    [Inject] private Player player;

    public override void Init()
    {
        base.Init();
        player.PlayerScoreController.OnScoreChange += PlayerScoreController_OnScoreChange;
    }

    private void OnDestroy()
    {
        player.PlayerScoreController.OnScoreChange -= PlayerScoreController_OnScoreChange;
    }

    protected override void BeforeShow()
    {
        scoreCounter.SetNumber(0, true);
    }

    private void PlayerScoreController_OnScoreChange(int score)
    {
        scoreCounter.SetNumber(score, true);
    }
}
