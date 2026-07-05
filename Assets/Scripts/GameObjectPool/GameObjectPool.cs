using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameObjectPool<T> where T : Component, IPoolable
{
    private readonly T _prefab;
    private readonly Transform parentContainer;
    private readonly Queue<T> pool = new();
    private readonly int _initialSize;

    public GameObjectPool(T prefab, int initialMaxSize, string poolName = null)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"{prefab} is null!");
        }
        _prefab = prefab;
        _initialSize = Mathf.Max(initialMaxSize, 1);
        string wrapperName = string.IsNullOrEmpty(poolName) ? $"{typeof(T).Name} Pool" : poolName;
        var wrapperGOP = new GameObject(wrapperName);
        parentContainer = wrapperGOP.transform;
        parentContainer.SetParent(null);
    }

    private void Prewarm()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            CreateNewGameObject();
            Debug.Log("добавили");
        }
    }
    private T CreateNewGameObject()
    {
        T obj = GameObject.Instantiate(_prefab, parentContainer);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }
    public T GetObject()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            Debug.LogWarning($"Pool {typeof(T).Name} is empty!");
            obj = CreateNewGameObject();
        }

        if (obj.gameObject.activeSelf == true)
        {
            Debug.LogWarning($"[GameObjectPool] {obj.gameObject.name} is already active!");
            return obj;
        }
        else
        {
            obj.gameObject.SetActive(true);
        }
        obj.OnEnable();
        return obj;
    }
    public void ReturnObject(T obj)
    {
        if (obj == null)
        {
            Debug.LogWarning($"Returned object is null!");
            return;
        }

        //obj.OnDisable();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parentContainer);
        pool.Enqueue(obj);
    }
    public void ClearPool()
    {
        while (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            if (obj != null)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }
    }
}