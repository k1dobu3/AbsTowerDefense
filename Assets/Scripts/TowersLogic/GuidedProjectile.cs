using UnityEngine;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class GuidedProjectile : BaseProjectile
	{
		private IDamageable _target;
		private AmmoSO _currentAmmoData;
		private GameObjectPool<GuidedProjectile> _pool;
		private Collider _collider;

		public void Initialize(AmmoSO ammoData, IDamageable target)
		{
			_currentAmmoData = ammoData;
			damage = _currentAmmoData.ammoDamage;
			_target = target;
		}
		
		public void SetPool(GameObjectPool<GuidedProjectile> pool) 
		{
			_pool = pool;
		}

		public override void OnSpawn()
		{
			if (_collider == null)
			{
				_collider = GetComponent<Collider>();			
			}
		}

		public override void OnDespawn()
		{
			_target = null;
			_pool.ReturnObject(this);
		}

		private void Update () {
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