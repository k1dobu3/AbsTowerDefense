using UnityEngine;

public interface IAim
{
    void AimTarget(bool rotateable, Transform _target, float projectileSpeed, float gunRotationSpeed);
    void AimReset(bool rotateable, Vector3 defaultRotationEuler, float gunRotationSpeed);
}
