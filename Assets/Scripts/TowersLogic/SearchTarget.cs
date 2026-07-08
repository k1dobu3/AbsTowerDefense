using UnityEngine;

public class SearchTarget : MonoBehaviour, ITargetable
{
    private float _distanceToActualTarget;

    public Transform FindTarget(Vector3 searcherPos, float fireRange)
    {
        Collider[] colliders = Physics.OverlapSphere(searcherPos, fireRange);
        Transform nearestTarget = null;


        foreach (var col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                float distance = (searcherPos - col.transform.position).magnitude;
                if (distance < fireRange)
                {
                    _distanceToActualTarget = distance;
                    nearestTarget = col.transform;
                }
                else
                {
                    _distanceToActualTarget = float.MaxValue;
                    nearestTarget = null;
                }
            }
        }

        return nearestTarget;
    }

    public float CalcTimeToTarget(float projectileSpeed)
    {
        return _distanceToActualTarget / projectileSpeed;
    }
}
