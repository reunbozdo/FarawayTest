using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class VisibilityAnimationController : MonoBehaviour, IShowable
{
    const string EDITOR_GROUP_NAME = "Animation settings";

    public enum State
    {
        none = 0,
        showAnimation = 1,
        hideAnimation = 2,
        showing = 3,
        hidden = 4
    }

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [SerializeField] private bool hideAtAwake = false;
    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [SerializeField] private bool overrideAnimationSettings;

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [ShowIf("overrideAnimationSettings")]
    [SerializeField] private AnimationSettings customAnimationSettings;

    [Space]

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [Header("Initial values")]
    [SerializeField] protected Vector3 initScale = Vector3.one;
    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [SerializeField] protected float initAlpha = 1f;

    [Inject] private AnimationSettings defaultAnimationSettings;

    protected AnimationSettings AnimationSettings => overrideAnimationSettings ? customAnimationSettings : defaultAnimationSettings;
    protected AnimationSettings.AnimationType AnimationType => AnimationSettings.animation;

    public State state { get; private set; }
    protected string animationID;

    protected void Awake()
    {
        state = State.showing;
        if (hideAtAwake) Hide(true);
    }

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [Button]
    public void Show(bool immediately = false, Action callback = null)
    {
        if (state == State.hideAnimation) StopAnimation();

        state = State.showAnimation;

        gameObject.SetActive(true);

        if (immediately)
        {
            transform.localScale = initScale;
            callback?.Invoke();
            state = State.showing;
            return;
        }

        HandleAnimation(true, AnimationSettings.animation, callback);
    }

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [Button]
    public void Hide(bool immediately = false, Action callback = null)
    {
        if (state == State.showAnimation) StopAnimation();

        state = State.hideAnimation;

        if (immediately)
        {
            gameObject.SetActive(false);
            state = State.hidden;
            transform.localScale = Vector3.zero;
            callback?.Invoke();
            return;
        }

        HandleAnimation(false, AnimationSettings.animation, callback);
    }

    public virtual void StopAnimation()
    {
        DOTween.Kill(animationID);
    }

    private void HandleAnimation(bool show, AnimationSettings.AnimationType animationType, Action callback)
    {
        if (!show) callback += () => gameObject.SetActive(false);

        State endState = show ? State.showing : State.hidden;
        callback += () => state = endState;

        switch (animationType)
        {
            case AnimationSettings.AnimationType.none:
                callback?.Invoke();
                break;

            case AnimationSettings.AnimationType.scale:
                animationID = "scale" + GetInstanceID();
                ScaleAnimation(show, callback);
                break;

            case AnimationSettings.AnimationType.fade:
                animationID = "fade" + GetInstanceID();
                FadeAnimation(show, callback);
                break;
        }
    }

    protected virtual void FadeAnimation(bool show, Action callback) { }

    protected virtual void ScaleAnimation(bool show, Action callback)
    {
        if (!show) initScale = transform.localScale;        

        transform.DOScale(show ? initScale : 0f.ToVector3(), AnimationSettings.duration)
                            .SetEase(show ? AnimationSettings.showCurve : AnimationSettings.hideCurve)
                            .OnComplete(() => callback?.Invoke())
                            .SetId(animationID);
    }

    [FoldoutGroup(EDITOR_GROUP_NAME)]
    [Button]
    private void GetInitScale()
    {
        initScale = transform.localScale;
    }
}
