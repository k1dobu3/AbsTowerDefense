using UnityEngine;

public class SearchTarget : MonoBehaviour, ITargetable
{
    private float _distanceToActualTarget;

    public GameObject FindTarget(Vector3 searcherPos, float fireRange)
    {
        Collider[] colliders = Physics.OverlapSphere(searcherPos, fireRange);
        GameObject nearestTarget = null;


        foreach (var col in colliders)
        {
            if (col.CompareTag("Monster"))
            {
                float distance = (searcherPos - col.transform.position).magnitude;
                if (distance < fireRange)
                {
                    _distanceToActualTarget = distance;
                    nearestTarget = col.gameObject;
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
}
