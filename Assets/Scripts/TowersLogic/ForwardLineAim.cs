using UnityEngine;

public class ForwardLineAim : MonoBehaviour, IAim
{
    public void AimTarget(bool rotateable, GameObject _target, float projectileSpeed, TowerDataSO currentTower)
    {
        if (rotateable)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    } 

    public void AimReset (bool rotateable, Vector3 defaultRotationEuler, TowerDataSO currentTower)
    {
        if (rotateable)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * currentTower.rotationSpeed);
        }
    }
}
