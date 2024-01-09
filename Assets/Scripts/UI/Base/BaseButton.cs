using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class BaseButton : UIElement, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private bool overrideButtonBehaviourSettings = false;
    [ShowIf("overrideButtonBehaviourSettings")]
    [SerializeField] private ButtonBehaviourSettings customButtonBehaviourSettings;

    [Inject] private ButtonBehaviourSettings defaultButtonBehaviourSettings;

    private UnityAction OnClick;
    private ButtonBehaviourSettings ButtonBehaviourSettings => overrideButtonBehaviourSettings ? customButtonBehaviourSettings : defaultButtonBehaviourSettings;

    public void SetOnClick(UnityAction action)
    {
        if (OnClick != null) Debug.Log("Button already had OnClick");
        OnClick = action;
        button.onClick.AddListener(OnClick);
    }

    public void RemoveOnClick()
    {
        if (OnClick == null) return;
        button.onClick.RemoveListener(OnClick);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ButtonBehaviourSettings.PressedState || !button.interactable) return;
        transform.DOScale(initScale * ButtonBehaviourSettings.PressedScaleMultiplier, ButtonBehaviourSettings.PressedStateDuration)
            .SetEase(ButtonBehaviourSettings.PressStateDownCurve);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!ButtonBehaviourSettings.PressedState || !button.interactable) return;

        transform.DOScale(initScale, ButtonBehaviourSettings.PressedStateDuration)
            .SetEase(ButtonBehaviourSettings.PressStateUpCurve);
    }

    void OnDestroy() => RemoveOnClick();


#if UNITY_EDITOR
    public override void Fill()
    {
        base.Fill();
        button = GetComponentInChildren<Button>();
    }
#endif
}
