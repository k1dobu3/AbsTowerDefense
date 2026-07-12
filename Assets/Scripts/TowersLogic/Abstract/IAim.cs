using UnityEngine;

public interface IAim
{
    void AimTarget(GameObject _target, float projectileSpeed, TowerDataSO currentTower);
    void AimReset(TowerDataSO currentTower);
}
