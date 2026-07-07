using UnityEngine;

public class ForwardLineAim : MonoBehaviour, IAim
{
    public void AimTarget(bool rotateable, Transform _target, float projectileSpeed, float gunRotationSpeed)
    {
        if (rotateable)
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
            Debug.Log("Навелся");
        }
    } 

    public void AimReset (bool rotateable, Vector3 defaultRotationEuler, float gunRotationSpeed)
    {
        if (rotateable)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * gunRotationSpeed);
        }
    }
}
