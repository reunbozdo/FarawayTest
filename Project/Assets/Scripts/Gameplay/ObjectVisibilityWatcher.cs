using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class ObjectVisibilityWatcher : MonoBehaviour
{
    public event Action<Transform> OnObjectNonVisible;

    [SerializeField] private float delayBetweenUpdatingVisibility = 2f;
    [SerializeField] private float offset = 0f;

    [Inject] private PlayerCameraPoint playerCameraPoint;

    public List<Transform> objects = new();

    IDisposable visibilitySystem;

    public void Init()
    {
        visibilitySystem = Observable
            .Interval(TimeSpan.FromSeconds(delayBetweenUpdatingVisibility))
            .Subscribe(UpdateVisibilityStateForObjects).AddTo(this);
    }

    private void OnDestroy()
    {
        visibilitySystem?.Dispose();
    }

    public void AddObjects(List<Transform> objs)
    {
        objects.AddRange(objs);
    }

    public void AddObject(Transform obj)
    {
        objects.Add(obj);
    }

    public void RemoveObject(Transform obj)
    {
        objects.Remove(obj);
    }

    private void UpdateVisibilityStateForObjects(long _)
    {
        List<Transform> nonVisibleObjects = objects.FindAll((obj) => playerCameraPoint.transform.position.z + offset > obj.position.z);

        foreach (Transform obj in nonVisibleObjects)
        {
            OnObjectNonVisible?.Invoke(obj);
        }
    }
}
