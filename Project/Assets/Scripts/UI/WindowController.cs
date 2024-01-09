using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WindowController : IInitializable
{
    enum State
    {
        none = 0,
        opening = 1,
        displayingWindow = 2,
        closing = 3
    }

    [Inject] private WindowControllerSettings settings;
    [Inject] private DiContainer diContainer;

    private State state = State.none;
    private BaseWindow CurrentWindow => windowHistory.Count == 0 ? null : windowHistory.LastObject();

    private List<BaseWindow> windowHistory;
    private List<BaseWindow> windowPrefabs;
    private Dictionary<Type, BaseWindow> windowDict = new();

    public void Initialize()
    {
        windowHistory = new List<BaseWindow>();
        windowPrefabs = settings.windowsInProject;
        foreach (BaseWindow window in windowPrefabs)
        {
            Type type = window.GetType();
            windowDict.Add(type, settings.createAllWindowsAtInitialize ? CreateWindow(type) : null);
        }
    }

    public T OpenWindow<T>(bool immediately = false, Action callback = null) where T : BaseWindow
    {
        if (state == State.closing || state == State.opening) return null;

        T window = GetWindow<T>();
        callback += () => state = State.displayingWindow;

        if (CurrentWindow == null)
        {
            state = State.opening;
            window.Show(immediately, callback);
            windowHistory.Add(window);
        }
        else
        {
            state = State.closing;
            CurrentWindow.Hide(immediately, () =>
            {
                state = State.opening;
                window.Show(immediately, callback);
                windowHistory.Add(window);
            });
        }
        return window;
    }

    public BaseWindow CloseLastWindow(bool immediately, Action callback = null)
    {
        if (state == State.closing || state == State.opening || CurrentWindow == null) return null;
        state = State.closing;
        callback += () => state = State.none;
        CurrentWindow.Hide(immediately, callback);
        return CurrentWindow;
    }

    public T GetWindow<T>() where T : BaseWindow
    {
        Type type = typeof(T);

        if (windowDict[type] == null)
            CreateWindow(type);

        return windowDict[type] as T;
    }

    private BaseWindow CreateWindow(Type type)
    {
        //BaseWindow createdWindow = GameObject.Instantiate(windowPrefabs.Find(x => x.GetType() == type));
        BaseWindow createdWindow = diContainer.InstantiatePrefabForComponent<BaseWindow>(windowPrefabs.Find(x => x.GetType() == type));
        windowDict[type] = createdWindow;
        createdWindow.Init();
        createdWindow.gameObject.SetActive(false);
        return createdWindow;
    }
}