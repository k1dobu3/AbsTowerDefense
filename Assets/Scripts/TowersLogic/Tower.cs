using UnityEngine;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
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


		private IDamageable _target;

		private void Awake()
		{
			_targetFinder = _targetFinderBehaviour as ITargetable;
			_targetAim = _aimBehaviour as IAim;
			_shooter = _shooterBehaviour as IShootable;
		}

		public void LateUpdate()
		{
			_target = _targetFinder.FindTarget(transform.position, _currentTowerData.fireRange);
			if (_target != null)
			{
				Vector3 predictedPos = _targetAim.GetPredictedPosition(_target, transform.position, _currentTowerData.startMuzzleSpeed);
        		_targetAim.AimTarget(_target, predictedPos, _currentTowerData);
				if (_targetAim.IsAimed)
				{
					_shooter.TryShoot(_currentTowerData.fireSpeedCD, _target, _currentTowerData.startMuzzleSpeed, predictedPos);
				}
			}
			else
			{
				_targetAim.AimReset(_currentTowerData);
			}

		}
	}
}
