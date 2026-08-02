using UnityEngine;
using System;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class GuidedProjectile : BaseProjectile
	{
		private IDamageable _target;
		private AmmoSO _currentAmmoData;
		private Collider _collider;

		public bool _isDespawned;

		public event Action<GuidedProjectile> OnDespawned;

		public override void Initialize(AmmoSO ammoData, IDamageable target)
		{
			_currentAmmoData = ammoData;
			damage = _currentAmmoData.ammoDamage;
			_target = target;
		}

		public override void OnSpawn()
		{
			if (_collider == null)
			{
				_collider = GetComponent<Collider>();			
			}
			_isDespawned = false;
		}

		public override void OnDespawn()
		{
			if (_isDespawned)
			{
				return;
			}
			_target = null;
			_isDespawned = true;
			OnDespawned?.Invoke(this);
		}

		private void Update () 
		{
			if (_isDespawned)
			{
				return;
			}
			if (_target == null || !_target.IsAlive) {
				OnDespawn();
				return;
			}

			var translation = _target.Transform.position - transform.position;
			if (translation.magnitude > _currentAmmoData.ammoSpeed) 
			{
				translation = translation.normalized * _currentAmmoData.ammoSpeed;
			}
			transform.Translate (translation);
		}
	}
}