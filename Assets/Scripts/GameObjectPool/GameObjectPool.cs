using System.Collections.Generic;
using UnityEngine;
using AbsTowerDefense.GameObjectPool.Abstract;

namespace AbsTowerDefense.GameObjectPool
{
	public class GameObjectPool<T> where T : Component, IPoolable
	{
		private readonly GameObject _prefab;
		private readonly Transform parentContainer;
		private readonly Queue<T> pool = new();
		private int _initialSize;
		private int _maxPoolSize;
		private int _currentTotalCount = 0; 

		public GameObjectPool(GameObject prefab, int initialMaxSize, int maxPoolSize, string poolName = null)
		{
			if (prefab == null)
			{
				Debug.LogError($"{prefab} is null");
				return;
			}
			_prefab = prefab;
			_initialSize = Mathf.Max(initialMaxSize, 1);
			_maxPoolSize = maxPoolSize == initialMaxSize ? maxPoolSize + 1 : maxPoolSize;
			string wrapperName = string.IsNullOrEmpty(poolName) ? $"{typeof(T).Name} Pool" : poolName;
			var wrapperGOP = new GameObject(wrapperName);
			parentContainer = wrapperGOP.transform;
			parentContainer.SetParent(null);
			Prewarm();
		}

		private void Prewarm()
		{
			for (int i = 0; i != _initialSize; i++)
			{
				T obj = CreateNewInstance();
				pool.Enqueue(obj);
			}
		}

		private T CreateNewInstance()
		{
			if (_currentTotalCount >= _maxPoolSize)
			{
				return null;
			}
			GameObject objGO = GameObject.Instantiate(_prefab, parentContainer);
			T obj = objGO.GetComponent<T>();
			obj.gameObject.SetActive(false);
			_currentTotalCount++;
			return obj;
		}

		public T GetObject()
		{
			T obj = null;
			if (pool.Count > 0)
			{
				obj = pool.Dequeue();
			}
			else
			{
				obj = CreateNewInstance();
			}
			if (obj != null)
			{
				obj.gameObject.SetActive(true);
				obj.OnSpawn();
			}
			return obj;
		}

		public void ReturnObject(T obj)
		{
			if (obj == null)
			{
				Debug.LogError($"Returned object is null!");
				return;
			}
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
			_currentTotalCount = 0;
		}
	}
}