using UnityEngine;

public interface IAim
{
    void AimTarget(bool rotateable, GameObject _target, float projectileSpeed, float gunRotationSpeed);
    void AimReset(bool rotateable, Vector3 defaultRotationEuler, float gunRotationSpeed);
}
