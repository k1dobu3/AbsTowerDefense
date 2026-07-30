using UnityEngine;
using AbsTowerDefense.GameObjectPool;

namespace AbsTowerDefense.TowersLogic
{
	public class CannonProjectile : BaseProjectile
	{
		private GameObjectPool<CannonProjectile> _pool;
		private AmmoSO _currentAmmoData;
		private float _spawnTime;

		[SerializeField]
		private Rigidbody _rb;

		public void Initialize(AmmoSO ammoData)
		{
			_currentAmmoData = ammoData;
			damage = _currentAmmoData.ammoDamage;
		}

		public void SetPool(GameObjectPool<CannonProjectile> pool) 
		{
			_pool = pool;
		}

		public override void OnSpawn()
		{
			_rb.linearVelocity = Vector3.zero;
			_rb.angularVelocity = Vector3.zero;
			_spawnTime = Time.time;
		}

		public override void OnDespawn()
		{
			_rb.linearVelocity = Vector3.zero;
			_rb.angularVelocity = Vector3.zero;
			_pool.ReturnObject(this);
		}

		public void SetVelocity(Vector3 Velocity)
		{
			_rb.linearVelocity = Velocity;
		}

		private void Update()
		{
			if (Time.time - _spawnTime > 5f)
			{
				OnDespawn();
			}
		}
	}
}