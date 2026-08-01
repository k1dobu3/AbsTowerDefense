using UnityEngine;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic.Abstract
{
	public interface IAim
	{
		bool IsAimed {get;}
		void AimTarget(IDamageable _target, Vector3 predictedPosition, TowerDataSO currentTower);
		void AimReset(TowerDataSO currentTower);
		Vector3 GetPredictedPosition(IDamageable target, Vector3 towerPosition, float projectileSpeed);
	}
}
