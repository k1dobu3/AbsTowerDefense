using UnityEngine;

public interface ITargetable
{
    GameObject FindTarget(Vector3 searcherPos, float fireRange);
    float CalcTimeToTarget(float projectileSpeed);
}
