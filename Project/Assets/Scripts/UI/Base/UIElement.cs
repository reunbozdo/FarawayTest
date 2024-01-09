using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using System.Linq;

public class UIElement : VisibilityAnimationController
{
    [SerializeField] protected List<Graphic> graphics;

    private int callbackTask;

    protected override void FadeAnimation(bool show, Action callback)
    {        
        foreach (Graphic graphic in graphics)
        {
            graphic.DOFade(show ? initAlpha : 0f, AnimationSettings.duration);
        }
        callbackTask = callback.Timer(AnimationSettings.duration);
    }

    public override void StopAnimation()
    {
        base.StopAnimation();
        DOTween.Kill(callbackTask);
    }

#if UNITY_EDITOR
    [Button("Fill")]
    public virtual void Fill()
    {
        graphics = GetComponentsInChildren<Graphic>().ToList();

        if (!graphics.IsEmpty())
            initAlpha = graphics.First().color.a;
    }
#endif
}
