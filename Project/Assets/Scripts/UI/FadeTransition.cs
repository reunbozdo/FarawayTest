using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float duration = 0.2f;
    [SerializeField] float delay = 0.2f;

    public void Transition(Action action)
    {
        image.gameObject.SetActive(true);

        Color ic = image.color;
        image.color = new Color(ic.r, ic.g, ic.b, 0f);

        image.DOFade(1f, duration).OnComplete(()=> {

            action?.Invoke();
            image.DOFade(0f, duration)
                 .SetDelay(delay)
                 .OnComplete(() => image.gameObject.SetActive(false));
        });
    }
}
