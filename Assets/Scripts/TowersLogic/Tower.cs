using System.Collections;
using UnityEngine;

public class Towers : MonoBehaviour, IPoolable
{
    [Header("Default Aim Line")]
    [SerializeField]
    private Vector3 defaultRotationEuler = new Vector3(0, 0, 0);
    [Header("Data of tower type")]
    [SerializeField]
    private TowerDataSO _data;

    private TowerDataSO _currentTower;
    private Transform _target;

    private void Awake()
    {
        if (_data == null)
        {
            Debug.Log($"Tower component found on {gameObject.name}");
            enabled = false;
            return;
        }

        Initaialize(_data);
        StartCoroutine(Shoot());
    }

    public void Initaialize(TowerDataSO data)
    {
        _currentTower = data;
    }

    public void Update()
    {
        if (!HasValidTarget())
        {
            FindTarget();
        }
        if (HasValidTarget())
        {
            if (_currentTower.towerGunHeadMoveable)
            {
                AimTarget();
            }
        }
        else
        {
            AimReset();
        }
    }

    private bool HasValidTarget()
    {
        if (_target == null)
        {
            return false;
        }
        if (_target.TryGetComponent<IDamageable>(out var damageable) && damageable.IsDead())
        {
            _target = null;
            return false;
        }
        return _target.gameObject.activeInHierarchy;
    }

    private void FindTarget()
    {
        _target = GetNearestMonsterInRange();
    }

    private Transform GetNearestMonsterInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _currentTower.fireRange);
        Transform nearestTarget = null;
        float minDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestTarget = col.transform;
                }
            }
        }
        return nearestTarget;
    }

    private void AimTarget()
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float targetY = targetRotation.eulerAngles.y;
            float currentY = transform.rotation.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentY, targetY, Time.deltaTime * _currentTower.rotationSpeed);

            transform.rotation = Quaternion.Euler(0, newY, 0);
        }
    }

    private void AimReset()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * _currentTower.rotationSpeed);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if  (GetNearestMonsterInRange() != null)
            {
                Instantiate(_currentTower.projectilePrefab, transform.position, transform.rotation); //заменить на пул, добавит поворот к цели
            }

            yield return new WaitForSeconds(_currentTower.fireSpeed);
        }
    }

    public void OnSpawn()
    {
    }

    public void OnDespawn()
    {
    }

    public Transform Transform => transform;
}
