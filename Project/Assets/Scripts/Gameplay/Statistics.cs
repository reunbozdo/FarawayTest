using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Statistics
{
    [Inject] private Player player;

    public int Highscore
    {
        get => PlayerPrefs.GetInt("Highscore");
        set => PlayerPrefs.SetInt("Highscore", value);
    }

    public int CurrentScore => player.PlayerScoreController.CurrentScore;
}
