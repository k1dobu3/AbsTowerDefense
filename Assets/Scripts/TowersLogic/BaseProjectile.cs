using UnityEngine;
using System;
using AbsTowerDefense.GameObjectPool.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public abstract class BaseProjectile : MonoBehaviour, IPoolable
	{
		public float damage;
		public abstract void OnSpawn();
		public abstract void OnDespawn();

		public virtual void Initialize(AmmoSO projectileData, IDamageable target = null)
		{
			throw new NotImplementedException();
		}
	}
}