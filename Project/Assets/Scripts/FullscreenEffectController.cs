using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullscreenEffectController : MonoBehaviour
{
    [SerializeField] private ScriptableRendererFeature pixelFeature;
    [SerializeField] private Material pixelMaterial;

    private string pixelTargetWidthName = "_TargetWidth";

    public void TurnOnPixelEffect(float duration, float targetWidth)
    {
        pixelFeature.SetActive(true);
        pixelMaterial.SetFloat(pixelTargetWidthName, Screen.width);
        pixelMaterial.DOFloat(targetWidth, pixelTargetWidthName, duration);
    }

    public void TurnOffPixelEffect(float duration)
    {
        pixelMaterial.DOFloat(Screen.width, pixelTargetWidthName, duration).OnComplete(() => pixelFeature.SetActive(false));
    }

    private void OnDestroy()
    {
        pixelFeature.SetActive(false);
        pixelMaterial.SetFloat(pixelTargetWidthName, Screen.width);
    }
}
