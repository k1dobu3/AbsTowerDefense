using UnityEngine;

namespace AbsTowerDefense.TowersLogic.Abstract
{
	public interface IShootable
	{
		float сurrentProjectileSpeed { get; }
		bool TryShoot(float firespeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos);
	}
}