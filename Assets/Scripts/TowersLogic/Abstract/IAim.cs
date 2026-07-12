using UnityEngine;

public interface IAim
{
    bool IsAimed {get;}
    void AimTarget(GameObject _target, float projectileSpeed, TowerDataSO currentTower);
    void AimReset(TowerDataSO currentTower);
}
