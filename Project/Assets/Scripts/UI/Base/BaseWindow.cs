using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour, IShowable
{
    protected enum State
    {
        none = 0,
        displayed = 1,
        showAnimation = 2,
        hideAnimation = 3
    }

    [SerializeField] private List<UIElement> elements;

    public virtual void Init() { }

    public void Show(bool immediately = false, Action callback = null)
    {
        BeforeShow();
        gameObject.SetActive(true);
        if (immediately)
        {
            foreach (var element in elements)
            {
                element.Hide(immediately);
            }
            AfterShow();
            callback?.Invoke();
        }
        else
        {
            int completedAnimationCount = 0;
            foreach (var element in elements)
            {
                element.Show(immediately, () =>
                {
                    completedAnimationCount++;
                    if (completedAnimationCount == elements.Count)
                    {
                        AfterHide();
                        callback?.Invoke();
                    }
                });
            }
        }
    }

    public void Hide(bool immediately = false, Action callback = null)
    {
        BeforeHide();
        if (immediately)
        {
            foreach (var element in elements)
            {
                element.Hide(immediately);
            }
            AfterHide();
            callback?.Invoke();
        }
        else
        {
            int completedAnimationCount = 0;
            foreach (var element in elements)
            {
                element.Hide(immediately, () =>
                {
                    completedAnimationCount++;
                    if (completedAnimationCount == elements.Count)
                    {
                        AfterHide();
                        callback?.Invoke();
                    }
                });
            }

        }
    }

    protected virtual void BeforeShow() { }

    protected virtual void AfterShow() { }

    protected virtual void BeforeHide() { }

    protected virtual void AfterHide() { }

#if UNITY_EDITOR
    [Button("Find all UI elements")]
    public virtual void Fill()
    {
        elements = GetComponentsInChildren<UIElement>().ToList();
    }
#endif
}
