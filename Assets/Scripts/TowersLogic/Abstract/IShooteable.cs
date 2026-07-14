using UnityEngine;

public interface IShooteable
{
    float сurrentProjectileSpeed { get; }
    void TryShoot(float firespeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos);
}
