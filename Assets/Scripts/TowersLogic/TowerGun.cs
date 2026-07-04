using UnityEngine;

public class TowerGun : ITowerStrategy
{
    private Towers _tower;
    private TowerDataSO _towerData;
    private Transform _target;

    public void Initialize(Towers tower, TowerDataSO towerData)
    {
        _tower = tower;
        _towerData = towerData;
    }

    public void Update()
    {
        if (_target == null)
        {
            FindTarget();
            return;
        }
        else
        {
            AimTarget();
            TryShootAtTarget();
        }
    }

    private void FindTarget()
    {
        // нужно доделать реализацию поиска цели!!!
    }

    private void AimTarget()
    {
        Vector3 direction = _target.position - _tower.Transform.position;
        direction.y = 0f;
        _tower.Transform.rotation = Quaternion.Lerp(_tower.Transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        Debug.Log("Rotating...");
    }

    private void TryShootAtTarget()
    {
        _towerData.fireSpeed -= Time.deltaTime;
        if (_towerData.fireSpeed <= 0f)
        {
            Shoot();
            _towerData.fireSpeed = 1f / _towerData.fireSpeed; // ?
        }
    }

    private void Shoot()
    {
        // нужно доделать реализацию стрельбы из пула!!!
    }

    public void OnSpawn()
    {
        
    }

    public void OnDestroy()
    {
        _target = null;
    }
}
