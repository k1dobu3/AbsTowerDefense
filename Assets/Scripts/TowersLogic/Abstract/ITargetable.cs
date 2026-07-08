using UnityEngine;

public interface ITargetable
{
    Transform FindTarget(Vector3 searcherPos, float fireRange);
    float CalcTimeToTarget(float projectileSpeed);
}
