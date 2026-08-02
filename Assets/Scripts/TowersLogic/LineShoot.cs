using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class LineShoot : MonoBehaviour, IShootable
	{
		private bool _canShoot = true;
		private GameObjectPool<GuidedProjectile> _pool;

		public float сurrentProjectileSpeed { get {return _currentAmmo.ammoSpeed;} }

		[SerializeField]
		private AmmoSO _currentAmmo;
		[SerializeField]
		private GameObject _halo;

		public bool TryShoot(float fireSpeed, IDamageable target, float startMuzzleSpeed, Vector3 predictedPos)
		{
			if (_canShoot && target != null)
			{
				MakeShoot(fireSpeed, target, this.GetCancellationTokenOnDestroy()).Forget();
				DisableHalo(fireSpeed * 0.2f, this.GetCancellationTokenOnDestroy()).Forget();
				return true;
			}
			return false;
		}

		private void Start()
		{
			_pool = PoolManager.Instance.CreateOrGetPool<GuidedProjectile>(_currentAmmo.ammoProjectilePrefab, 5, $"{_currentAmmo.ammoName}");
			SpawnProjectile();
		}

		private void SpawnProjectile(IDamageable target = null)
		{
			GuidedProjectile crystal = _pool.GetObject();
			if (crystal != null)
			{
				crystal.transform.position = transform.position;
				crystal.Initialize(_currentAmmo, target);
				crystal.SetPool(_pool);
			}
		}

		private async UniTaskVoid MakeShoot(float firespeed, IDamageable target, CancellationToken cancellationToken)
		{
			_canShoot = false;
			SpawnProjectile(target);
			await UniTask.Delay((int)(firespeed * 1000), cancellationToken: cancellationToken);
			_canShoot = true;
		}

		private async UniTaskVoid DisableHalo(float sparklesDisableTime, CancellationToken cancellationToken)
		{
			if (_halo == null)
			{
				return;
			}
			_halo.SetActive(false);
			await UniTask.Delay((int)(sparklesDisableTime * 1000), cancellationToken: cancellationToken);
			_halo.SetActive(false);
		}
	}
}