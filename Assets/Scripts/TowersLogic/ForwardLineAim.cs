using UnityEngine;

public class ForwardLineAim : MonoBehaviour, IAim
{
    [Header("Default Aim Line")]
    [SerializeField] private Vector3 defaultRotationEuler = new Vector3(0, 0, 0);
    public bool IsAimed => true;
    public void AimTarget(GameObject _target, float projectileSpeed, TowerDataSO currentTower)
    {
        if (currentTower.towerGunHeadMoveable)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    } 

    public void AimReset (TowerDataSO currentTower)
    {
        if (currentTower.towerGunHeadMoveable)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * currentTower.rotationSpeed);
        }
    }

    public float CalculateFlightTime(Vector3 start, Vector3 target, float speed)
    {
        return 0;
    }
}
