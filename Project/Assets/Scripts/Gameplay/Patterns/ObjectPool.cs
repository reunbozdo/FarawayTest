using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : VisibilityAnimationController
{    
    [SerializeField] protected T prefab;
    [SerializeField] protected int poolSize = 10;
    [SerializeField] protected Transform parent;

    [ShowInInspector] public List<T> PooledObjects { private set; get; }

    private bool initialized = false;

    public virtual void Init()
    {        
        PooledObjects = new List<T>();
        initialized = true;
    }

    public virtual T GetObject()
    {
        if (!initialized) Init();

        var obj = PooledObjects.Find((obj) => obj.state == VisibilityAnimationController.State.hidden);
        if (obj != null)
        {
            obj.Show(true);
            return obj;
        }
        
        return CreateObject();
    }

    private T CreateObject()
    {
        if (poolSize < PooledObjects.Count + 1)
            RemoveObject(PooledObjects.First());

        T newObj = Instantiate(prefab, parent);
        newObj.Show(true);
        PooledObjects.Add(newObj);
        return newObj;
    }

    public void RemoveObject(T obj)
    {
        PooledObjects.Remove(obj);
        Destroy(obj.gameObject);
    }
}