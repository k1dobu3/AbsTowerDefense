using UnityEngine;

public class Towers : MonoBehaviour
{
    [Header("Default Aim Line")]
    [SerializeField]
    private Vector3 defaultRotationEuler = new Vector3(0, 0, 0);
    [Header("Data of tower type")]
    [SerializeField]
    private TowerDataSO _data;

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
        if (!_target)
        {
            _timeToTarget = _targetFinder.CalcTimeToTarget(_projectileSpeed);
        }
        if (_target)
        {
            if (_currentTower.towerGunHeadMoveable)
            {
                _targetAim.AimTarget(_currentTower.towerGunHeadMoveable, _target, _projectileSpeed, _currentTower);
                _shooter.TryShoot(_currentTower.fireSpeedCD, _target);
            }
        }
        else
        {
            _targetAim.AimReset(_currentTower.towerGunHeadMoveable, defaultRotationEuler, _currentTower);
        }
    }

    public Transform Transform => transform;
}
