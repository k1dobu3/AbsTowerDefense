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

    public void TryShoot(float fireSpeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos)
    {
        if (_canShoot )
        {
            StartCoroutine(MakeShoot(fireSpeed, target, startMuzzleSpeed, predictedPos));
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
        // Debug.Log("Пиу")
        CannonProjectile projectile = SpawnProjectile();
        Debug.LogError("1");
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

        Debug.Log($"Projectile = {projectile}");
        Debug.Log($"ShootPoint = {_shootStartPoint}");
        projectile.transform.SetPositionAndRotation(_shootStartPoint.position, Quaternion.LookRotation(shootDirection));
        projectile.rb.linearVelocity = shootDirection * startMuzzleSpeed;        
        
        yield return new WaitForSeconds(fireSpeed);
        _canShoot = true;
    }

}
