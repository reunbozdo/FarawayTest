using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] protected List<AnimationInfo> animationInfos;
    [SerializeField] protected AnimancerComponent animancer;

    public AnimancerState PlayAnimation(string animationName, float fadeTime = 0f, Action callback = null)
    {
        var animationInfo = animationInfos.Find((x) => x.name == animationName);

        if (animationInfo == null) return null;

        var state = animancer.Play(animationInfo.clip, fadeTime);
        state.Speed = animationInfo.speed;

        callback += ()=> state.Events.OnEnd -= callback; //invoke callback only once after animation end (by default it every frame after animation end).
        state.Events.OnEnd += callback;

        return state;
    }

    public AnimancerState PlayReversedAnimation(string animationName, float fadeTime = 0f, Action callback = null)
    {
        var animationInfo = animationInfos.Find((x) => x.name == animationName);

        if (animationInfo == null) return null;

        var state = animancer.Play(animationInfo.clip, fadeTime);
        state.Speed = -animationInfo.speed;

        callback += () => state.Events.OnEnd -= callback;
        state.Events.OnEnd += callback;

        return state;
    }
}
