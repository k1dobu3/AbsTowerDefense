using System.Collections;
using UnityEngine;

public class LineShoot : MonoBehaviour, IShooteable
{
    [SerializeField] public AmmoSO _currentAmmo;
    [SerializeField] public GameObject _halo;
    private bool _canShoot = true;
    private GameObjectPool<GuidedProjectile> _pool;

    public float CurrentProjectileSpeed
    {
        get
        {
            return _currentAmmo.ammoSpeed;
        }
    }

    public void Start()
    {
        if (_pool == null)
        {
            _pool = new GameObjectPool<GuidedProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CrystalTower");
        }
        SpawnProjectile();
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

    public void TryShoot(float fireSpeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos)
    {
        if (_canShoot && target != null)
        {
            StartCoroutine(MakeShoot(fireSpeed, target));
            StartCoroutine(DisableHalo(fireSpeed*0.8f));
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

    private void OnDestroy()
    {
        if (_pool != null)
        {
            _pool.ClearPool();
        }
    }
}
