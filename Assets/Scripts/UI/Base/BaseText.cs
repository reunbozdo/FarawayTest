using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BaseText : UIElement
{
    [SerializeField] private Text text;
    [SerializeField] private bool overrideTextBehaviourSettings;
    [ShowIf("overrideTextBehaviourSettings")]
    [SerializeField] private TextBehaviourSettings customTextBehaviourSettings;

    [Inject] private TextBehaviourSettings defaultTextBehaviourSettings;

    private TextBehaviourSettings TextBehaviourSettings => overrideTextBehaviourSettings ? customTextBehaviourSettings : defaultTextBehaviourSettings;

    private int currentNumber;
    private string numberCountID => $"count{GetInstanceID()}";

    public void SetNumber(int number, bool immediately = false, string prefix = "", string postfix = "", Action callback = null)
    {
        if (immediately)
        {
            text.text = $"{prefix}{number}{postfix}";
            currentNumber = number;
            callback?.Invoke();
            return;
        }

        DOTween.Kill(numberCountID);

        DOTween.To(() => currentNumber, (x) => currentNumber = x, number, TextBehaviourSettings.durationOfNumberCounting)
            .OnUpdate(() =>
            {
                text.text = $"{prefix}{currentNumber}{postfix}";
            })
            .OnComplete(() =>
            {
                callback?.Invoke();
            })
            .SetId(numberCountID);
    }

    public void SetText(string message)
    {
        if (text == null) return;
        text.text = message;
    }
}
