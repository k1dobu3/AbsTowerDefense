using UnityEngine;

public interface IAim
{
    bool IsAimed {get;}
    void AimTarget(GameObject _target, Vector3 predictedPosition, TowerDataSO currentTower);
    void AimReset(TowerDataSO currentTower);
    float CalculateFlightTime(Vector3 start, Vector3 target, float speed);
    Vector3 GetPredictedPosition(GameObject target, Vector3 towerPosition, float projectileSpeed); //Test
}
