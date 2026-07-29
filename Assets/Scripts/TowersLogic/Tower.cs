using UnityEngine;

public class Tower : MonoBehaviour
{
	[Header("Data of tower type")]
	[SerializeField] 
	private TowerDataSO _currentTowerData;
	[SerializeField]
	private MonoBehaviour _targetFinderBehaviour;
	[SerializeField]
	private MonoBehaviour _aimBehaviour;
	[SerializeField]
	private MonoBehaviour _shooterBehaviour;

	private ITargetable _targetFinder;
	private IAim _targetAim;
	private IShootable _shooter;


	private GameObject _target;

	private void Awake()
	{
		_targetFinder = _targetFinderBehaviour as ITargetable;
		_targetAim = _aimBehaviour as IAim;
		_shooter = _shooterBehaviour as IShootable;
	}

	public void LateUpdate()
	{
		_target = _targetFinder.FindTarget(transform.position, _currentTowerData.fireRange);
		if (_target)
		{
			Vector3 predictedPostion = _targetAim.GetPredictedPosition(_target, transform.position, _currentTowerData.startMuzzleSpeed);
			_targetAim.AimTarget(_target, predictedPostion, _currentTowerData);
			if (_targetAim.IsAimed)
			{
				Vector3 predictedPos = _targetAim.GetPredictedPosition(_target, transform.position, _currentTowerData.startMuzzleSpeed);
				_shooter.TryShoot(_currentTowerData.fireSpeedCD, _target, _currentTowerData.startMuzzleSpeed, predictedPos);
			}
		}
		else
		{
			_targetAim.AimReset(_currentTowerData);
		}
	}
}
