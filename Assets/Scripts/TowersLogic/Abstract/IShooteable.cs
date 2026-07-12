using UnityEngine;

public interface IShooteable
{
    float CurrentProjectileSpeed {get;}
    void TryShoot(float firespeed, GameObject target, float startMuzzleSpeed, Vector3 predictedPos);
}
