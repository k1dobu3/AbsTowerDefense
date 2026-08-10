using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class BallisticShoot : MonoBehaviour, IShootable
	{
		private GameObjectPool<CannonProjectile> _pool;
		private ProjectileFactory<CannonProjectile> _projectileFactory;
		private bool _canShoot = true;

		public float сurrentProjectileSpeed { get { return _currentAmmo.ammoSpeed; } }

		[SerializeField]
		private AmmoSO _currentAmmo;
		[SerializeField]
		private int _maxPoolSize = 10;
		[SerializeField]
		private Transform _shootStartPoint;
		[SerializeField]
		private GameObject _sparkles;

		public bool TryShoot(float fireSpeed, IDamageable target, float startMuzzleSpeed, Vector3 predictedPos)
		{
			if (_canShoot)
			{
				MakeShoot(fireSpeed, target, startMuzzleSpeed, this.GetCancellationTokenOnDestroy(), predictedPos).Forget();
				DisableSparkles(fireSpeed * 0.8f, this.GetCancellationTokenOnDestroy()).Forget();
				return true;
			}
			return false;
		}

		private void Awake()
		{
			_pool = PoolManager.Instance.CreateOrGetPool<CannonProjectile>(_currentAmmo.ammoProjectilePrefab, 5, _maxPoolSize, $"{_currentAmmo.ammoName}");
			_projectileFactory = new ProjectileFactory<CannonProjectile>(_pool, _currentAmmo);
		}

		private void OnProjectileDespawned(CannonProjectile projectile)
		{
			projectile.OnDespawned -= OnProjectileDespawned;
			_projectileFactory.ReturnToPool(projectile);
		}

		private async UniTaskVoid MakeShoot(float firespeed, IDamageable target, float startMuzzleSpeed, CancellationToken cancellationToken, Vector3? predictedPos = null)
		{
			_canShoot = false;
			CannonProjectile projectile = _projectileFactory.CreateProjectile();
			if (projectile == null)
			{
				Debug.LogWarning("Выстрел отменен: projectile == null");
				_canShoot = true;
				return;
			}
			Vector3 shootDirection = _shootStartPoint.forward;

			projectile.transform.SetPositionAndRotation(_shootStartPoint.position, Quaternion.LookRotation(shootDirection));
			projectile.SetVelocity(shootDirection * startMuzzleSpeed);

			projectile.OnDespawned += OnProjectileDespawned;

			await UniTask.Delay((int)(firespeed * 1000), cancellationToken: cancellationToken);
			_canShoot = true;
		}

		private async UniTaskVoid DisableSparkles(float sparklesDisableTime, CancellationToken cancellationToken)
		{
			if (_sparkles == null)
			{
				return;
			}
			_sparkles.SetActive(false);
			await UniTask.Delay((int)(sparklesDisableTime * 1000), cancellationToken: cancellationToken);
			_sparkles.SetActive(true);
		}
	}
}