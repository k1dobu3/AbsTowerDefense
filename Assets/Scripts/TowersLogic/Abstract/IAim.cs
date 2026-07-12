using UnityEngine;

public interface IAim
{
    void AimTarget(bool rotateable, GameObject _target, float projectileSpeed, TowerDataSO currentTower);
    void AimReset(bool rotateable, Vector3 defaultRotationEuler, TowerDataSO currentTower);
}
