using UnityEngine;

public class Towers : MonoBehaviour
{
    [Header("Data of tower type")]
    [SerializeField] private TowerDataSO _data;

    private TowerDataSO _currentTower;
    private GameObject _target;
    private float _projectileSpeed;
    private float _timeToTarget;
    private IShooteable _shooter;
    private ITargetable _targetFinder;
    private IAim _targetAim;

    private void Awake()
    {
        if (_data == null)
        {
            Debug.Log($"Tower component found on {gameObject.name}");
            enabled = false;
            return;
        }
        Initaialize(_data);
    }

    public void Initaialize(TowerDataSO data)
    {
        _currentTower = data;
        _shooter = GetComponent<IShooteable>();
        _targetFinder = GetComponent<ITargetable>();
        _targetAim = GetComponent<IAim>();
        _projectileSpeed = _shooter.CurrentProjectileSpeed;
    }

    public void LateUpdate()
    {
        _target = _targetFinder.FindTarget(transform.position, _currentTower.fireRange);
        if (_target)
        {
            _timeToTarget = _targetFinder.CalcTimeToTarget(_projectileSpeed);
            _targetAim.AimTarget(_target, _projectileSpeed, _currentTower);
            Debug.Log(_targetAim.IsAimed);
            if (_targetAim.IsAimed)
            {   
                _shooter.TryShoot(_currentTower.startMuzzleSpeed, _target);
            }
        }

        if (_target == null)
        {
            _targetAim.AimReset(_currentTower);
        }
    }

    public Transform Transform => transform;
}
