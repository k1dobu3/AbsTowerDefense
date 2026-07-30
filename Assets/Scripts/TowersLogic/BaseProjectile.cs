using UnityEngine;
using AbsTowerDefense.GameObjectPool.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public abstract class BaseProjectile : MonoBehaviour, IPoolable
	{
		public float damage;
		public abstract void OnSpawn();
		public abstract void OnDespawn();
	}
}