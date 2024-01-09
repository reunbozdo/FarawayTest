using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerScoreController : MonoBehaviour
{
    public event Action<int> OnScoreChange;

    [Inject] private Player player;
    [Inject] private Statistics statistics;

    public int CurrentScore
    {
        get => currentScore;
        set
        {
            if (currentScore != value)
            {
                currentScore = value;
                OnScoreChange?.Invoke(currentScore);
            }
        }
    }

    private int currentScore;        

    public void Init()
    {
        player.OnObstacleHit += Player_OnObstacleHit;
    }

    private void OnDestroy()
    {
        player.OnObstacleHit -= Player_OnObstacleHit;
    }

    private void FixedUpdate()
    {
        CurrentScore = (int)(transform.position.z * 10f);
    }

    private void Player_OnObstacleHit()
    {        
        if (CurrentScore > statistics.Highscore)
        {
            statistics.Highscore = CurrentScore;
        }
    }
}
