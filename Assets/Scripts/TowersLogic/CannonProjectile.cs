using UnityEngine;
using System;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class CannonProjectile : BaseProjectile
	{
		private AmmoSO _currentAmmoData;
		private float _spawnTime;

		[SerializeField]
		private Rigidbody _rb;

		public bool _isDespawned;

		public event Action<CannonProjectile> OnDespawned;

		public override void Initialize(AmmoSO ammoData, IDamageable target = null)
		{
			_currentAmmoData = ammoData;
			damage = _currentAmmoData.ammoDamage;
		}

		public override void OnSpawn()
		{
			_spawnTime = Time.time;
			_isDespawned = false;
		}

		public override void OnDespawn()
		{
			if (_isDespawned)
			{
				return;
			}
			_isDespawned = true;
			OnDespawned?.Invoke(this);
		}

		public void SetVelocity(Vector3 Velocity)
		{	
			if (_rb != null)
			{
				_rb.linearVelocity = Velocity;
			}
		}

		private void Update()
		{
			if (_isDespawned)
			{
				return;
			}
			if (Time.time - _spawnTime > 5f)
			{
				OnDespawn();
			}
		}
	}
}