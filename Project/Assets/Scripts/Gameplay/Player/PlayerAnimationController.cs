using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using System;
using UniRx;
using DG.Tweening;

public class PlayerAnimationController : AnimationController
{
    enum State
    {
        none = 0,
        idle = 1,
        running = 2,
        jump = 3,
        crawl = 4,
        fly = 5
    }

    private State state = State.none;

    private PlayerSpeedController playerSpeedController;
    private AnimancerState currentAnimationState;

    IDisposable animationSpeedAdjustmentSystem;

    public void Init(PlayerSpeedController playerSpeedController)
    {
        this.playerSpeedController = playerSpeedController;

        animationSpeedAdjustmentSystem = Observable.EveryFixedUpdate()
            .Subscribe((_) => AdjustRunAnimationSpeed());
    }

    private void OnDestroy()
    {
        animationSpeedAdjustmentSystem?.Dispose();
    }

    public void Idle(float changeDuration)
    {
        state = State.idle;
        currentAnimationState = PlayAnimation("Idle", changeDuration);
    }

    public void StartRun(float changeDuration)
    {
        state = State.running;
        currentAnimationState = PlayAnimation("FastRun", changeDuration);
    }

    int jumpTaskID;

    public void Jump(float changeDuration, float jumpDuration, Action callback = null)
    {
        animancer.transform.localEulerAngles = Vector3.zero;
        animancer.transform.localPosition = Vector3.zero;

        float clipLenght = animationInfos.Find((x) => x.name == "SimpleJump").clip.length;

        state = State.jump;   
        currentAnimationState = PlayAnimation("SimpleJump", changeDuration);

        Action OnJumpEnd = () => currentAnimationState = PlayReversedAnimation("SimpleJump", changeDuration, callback);        
        jumpTaskID = OnJumpEnd.Timer(jumpDuration - clipLenght);
    }

    int crawlTaskID;

    public void Crawl(float changeDuration, float crawlDuration, Action callback = null)
    {
        //animancer.transform.localEulerAngles = Vector3.zero;
        animancer.transform.localPosition = Vector3.zero;

        state = State.crawl;

        currentAnimationState = PlayAnimation("Crawl", changeDuration);

        Action afterCrawl = () =>
        {
            state = State.none;
            StartRun(changeDuration);
            //animancer.transform.localEulerAngles = Vector3.zero;
            animancer.transform.localPosition = Vector3.zero;
            callback?.Invoke();
        };

        crawlTaskID = afterCrawl.Timer(crawlDuration);
    }

    int flyTaskID;

    public void Fly(float changeDuration)
    {
        currentAnimationState = PlayAnimation("Fly", changeDuration);
        state = State.fly;

        //Action afterFly = () =>
        //{
        //    state = State.none;
        //    StartRun(changeDuration);
        //};

        //flyTaskID = afterFly.Timer(flyDuration);
    }

    public void Roll(float changeDuration, Action callback = null)
    {
        currentAnimationState = PlayAnimation("Roll", changeDuration, callback);
    }

    private void AdjustRunAnimationSpeed()
    {
        if (state == State.running)
            currentAnimationState.Speed = playerSpeedController.NormalizedSpeed;
    }

    public void StopCurrentAnimation()
    {
        DOTween.Kill(flyTaskID);
        DOTween.Kill(crawlTaskID);
        DOTween.Kill(jumpTaskID);
        currentAnimationState.Stop();
    }
}
