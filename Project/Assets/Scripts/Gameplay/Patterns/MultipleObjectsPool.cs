using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using Zenject;

public class MultipleObjectsPool<T> : MonoBehaviour where T : VisibilityAnimationController
{
    [System.Serializable]
    public class ObjectInfo
    {
        public int index;
        public T obj;

        public ObjectInfo(int index, T obj)
        {
            this.index = index;
            this.obj = obj;
        }
    }

    [SerializeField] protected List<T> prefabs;
    [SerializeField] protected int poolSize = 10;
    [SerializeField] protected Transform parent;

    [Inject] private DiContainer diContainer;

    public List<T> Prefabs => prefabs;

    [ShowInInspector] public List<ObjectInfo> PooledObjects { private set; get; }

    private bool initialized = false;

    public virtual void Init()
    {
        PooledObjects = new();
        initialized = true;
    }

    public virtual T GetRandomObject(bool showImmediately = true)
    {
        if (!initialized) Init();

        int index = prefabs.Count.Random();
        return GetObject(index, showImmediately);
    }

    public virtual T GetObject(int index, bool immediately = true)
    {
        if (!initialized) Init();

        var objInfo = PooledObjects.Find((objInfo) => objInfo.obj.state == VisibilityAnimationController.State.hidden && objInfo.index == index);        
        if (objInfo != null)
        {
            var obj = objInfo.obj;
            obj.Show(immediately);
            return obj;
        }

        return CreateObject(index, immediately);
    }

    public virtual T GetObject(T obj)
    {
        if (!initialized) Init();

        int index = prefabs.FindIndex((x) => obj == x);
        return GetObject(index);
    }

    private T CreateObject(int index, bool immediately = true)
    {
        if (index < 0 || index > prefabs.Count) return null;

        if (poolSize < PooledObjects.Count + 1)
            RemoveObject(PooledObjects.First());

        var prefab = prefabs[index];
        //T newObj = Instantiate(prefab, parent);
        T newObj = diContainer.InstantiatePrefabForComponent<T>(prefab, parent);
        newObj.Show(immediately);
        PooledObjects.Add(new ObjectInfo(index, newObj));
        return newObj;
    }

    public void RemoveObject(ObjectInfo objInfo)
    {
        PooledObjects.Remove(objInfo);
        Destroy(objInfo.obj.gameObject);
    }
}
