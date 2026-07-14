using System;
using UnityEngine;

public class Towers : MonoBehaviour
{
    [Header("Data of tower type")]
    [SerializeField] 
    private TowerDataSO _data;

    private TowerDataSO _currentTower;
    private GameObject _target;
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
    }

    public void FixedUpdate()
    {
        _target = _targetFinder.FindTarget(transform.position, _currentTower.fireRange);
        if (_target)
        {
            Vector3 predictedPostion = _targetAim.GetPredictedPosition(_target, transform.position, _currentTower.startMuzzleSpeed);
            _targetAim.AimTarget(_target, predictedPostion, _currentTower);
            // Debug.Log(_targetAim.IsAimed);
            if (_targetAim.IsAimed)
            {
                Vector3 predictedPos = _targetAim.GetPredictedPosition(_target, transform.position, _currentTower.startMuzzleSpeed);
                _shooter.TryShoot(_currentTower.fireSpeedCD, _target, _currentTower.startMuzzleSpeed, predictedPos);
            }
        }

        if (_target == null)
        {
            _targetAim.AimReset(_currentTower);
        }
    }
}
