using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallisticShoot : MonoBehaviour, IShooteable
{
    [SerializeField] public AmmoSO _currentAmmo;
    [SerializeField] public Transform _shootStartPoint;
    private GameObjectPool<CannonProjectile> _pool;
    private bool _canShoot = true;

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
            _pool = new GameObjectPool<CannonProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CannonTower");
        }
    }

    public void TryShoot(float fireSpeed, GameObject target)
    {
        if (_canShoot )
        {
            StartCoroutine(MakeShoot(fireSpeed, target));
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

    private IEnumerator MakeShoot(float fireSpeed, GameObject target)
    {
        _canShoot = false;
        Debug.Log("Пиу");
        CannonProjectile projectile = SpawnProjectile();
        if (projectile == null)
        {
            Debug.LogError("Выстрел отменен: projectile == null");
            _canShoot = true;
            yield break;
        }
        // Vector3 velocity = CalculateBallisticVelocity(_shootStartPoint.position, target.transform.position, fireSpeed);
        Debug.Log($"Projectile = {projectile}");
        Debug.Log($"ShootPoint = {_shootStartPoint}");
        projectile.transform.SetPositionAndRotation(_shootStartPoint.position, _shootStartPoint.rotation);
        projectile.rb.linearVelocity =_shootStartPoint.forward * fireSpeed;        
        
        yield return new WaitForSeconds(fireSpeed);
        _canShoot = true;
    }

}
