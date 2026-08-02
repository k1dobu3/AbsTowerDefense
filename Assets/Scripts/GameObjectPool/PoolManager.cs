using UnityEngine;
using AbsTowerDefense.GameObjectPool.Abstract;
using System.Collections.Generic;

namespace AbsTowerDefense.GameObjectPool
{
	public class PoolManager : MonoBehaviour
	{
		public static PoolManager Instance {get; private set; }

		private readonly Dictionary<GameObject, object> _pools = new();

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
		}

		public GameObjectPool<T> CreateOrGetPool<T>(GameObject prefab, int size = 5, string name = "Pool") where T : Component, IPoolable
		{
			if (prefab == null)
			{
				return null;
			}

			if (_pools.TryGetValue(prefab, out var existing))
			{
				return existing as GameObjectPool<T>;
			}

			var pool = new GameObjectPool<T>(prefab, size, name ?? prefab.name);
			_pools[prefab] = pool;
			return pool;
		}

		private void OnDestroy()
		{
			_pools.Clear();
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}
}