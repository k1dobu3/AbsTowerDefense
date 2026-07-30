using UnityEngine;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic.Abstract
{
	public interface IShootable
	{
		float сurrentProjectileSpeed { get; }
		bool TryShoot(float firespeed, IDamageable target, float startMuzzleSpeed, Vector3 predictedPos);
	}
}