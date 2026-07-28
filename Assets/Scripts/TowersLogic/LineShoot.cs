using System.Collections;
using UnityEngine;

public class LineShoot : MonoBehaviour, IShootable
{
	private bool _canShoot = true;
	private GameObjectPool<GuidedProjectile> _pool;

	public float сurrentProjectileSpeed { get {return _currentAmmo.ammoSpeed;} }

	[SerializeField]
	private AmmoSO _currentAmmo;
	[SerializeField]
	private GameObject _halo;

	public bool TryShoot(float fireSpeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos)
	{
		if (_canShoot && target != null)
		{
			StartCoroutine(MakeShoot(fireSpeed, target));
			StartCoroutine(DisableHalo(fireSpeed*0.8f));
			return true;
		}
		return false;
	}

	private void Awake()
	{
		if (_pool == null)
		{
			_pool = new GameObjectPool<GuidedProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CrystalTower");
		}
	}

	private void Start()
	{
		SpawnProjectile();
	}

	private void OnDestroy()
	{
		if (_pool != null)
		{
			_pool.ClearPool();
		}
	}

	private void SpawnProjectile(GameObject target = null)
	{
		GuidedProjectile crystal = _pool.GetObject();
		if (crystal != null)
		{
			crystal.transform.position = transform.position;
			crystal.Initialize(_currentAmmo, target);
			crystal.SetPool(_pool);
		}
	}

	private IEnumerator MakeShoot(float fireSpeed, GameObject target)
	{
		_canShoot = false;
		SpawnProjectile(target);
		yield return new WaitForSeconds(fireSpeed);
		_canShoot = true;
	}

	private IEnumerator DisableHalo(float haloDisableTime)
	{
		if (_halo == null)
		{
			yield break;
		}
		_halo.SetActive(false);
		yield return new WaitForSeconds(haloDisableTime);
		_halo.SetActive(true);
	}
}
