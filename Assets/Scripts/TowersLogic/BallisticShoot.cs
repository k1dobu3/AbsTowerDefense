using System.Collections;
using UnityEngine;

public class BallisticShoot : MonoBehaviour, IShootable
{
	private GameObjectPool<CannonProjectile> _pool;
	private bool _canShoot = true;

	public float сurrentProjectileSpeed { get { return _currentAmmo.ammoSpeed; } }

	[SerializeField]
	private AmmoSO _currentAmmo;
	[SerializeField]
	private Transform _shootStartPoint;
	[SerializeField]
	private GameObject _sparkles;

	public bool TryShoot(float fireSpeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos)
	{
		if (_canShoot)
		{
			StartCoroutine(MakeShoot(fireSpeed, target, startMuzzleSpeed, predictedPos));
			StartCoroutine(DisableSparkles(fireSpeed * 0.2f));
			return true;
		}
		return false;
	}

	private void Awake()
	{
		if (_pool == null)
		{
			_pool = new GameObjectPool<CannonProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CannonTower");
		}
	}

	private void OnDestroy()
	{
		if (_pool != null)
		{
			_pool.ClearPool();
		}
	}

	private CannonProjectile SpawnProjectile()
	{
		CannonProjectile projectile = _pool.GetObject();
		if (projectile == null)
		{
			return null;
		}
		projectile.Initialize(_currentAmmo);
		projectile.SetPool(_pool);
		return projectile;
	}

	private IEnumerator MakeShoot(float fireSpeed, GameObject target, float startMuzzleSpeed, Vector3? predictedPos = null)
	{
		_canShoot = false;
		CannonProjectile projectile = SpawnProjectile();
		if (projectile == null)
		{
			Debug.LogError("Выстрел отменен: projectile == null");
			_canShoot = true;
			yield break;
		}

		Vector3 shootDirection;
		if (predictedPos.HasValue)
		{
			shootDirection = (predictedPos.Value - _shootStartPoint.position).normalized;
		}
		else
		{
			shootDirection = _shootStartPoint.forward;
		}

		projectile.transform.SetPositionAndRotation(_shootStartPoint.position, Quaternion.LookRotation(shootDirection));
		projectile.SetVelocity(shootDirection * startMuzzleSpeed);

		yield return new WaitForSeconds(fireSpeed);
		_canShoot = true;
	}

	private IEnumerator DisableSparkles(float sparklesDisableTime)
	{
		if (_sparkles == null)
		{
			yield break;
		}
		_sparkles.SetActive(false);
		yield return new WaitForSeconds(sparklesDisableTime);
		_sparkles.SetActive(true);
	}
}
