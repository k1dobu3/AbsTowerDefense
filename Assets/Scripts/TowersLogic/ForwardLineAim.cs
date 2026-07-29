using UnityEngine;

public class ForwardLineAim : MonoBehaviour, IAim
{
	[Header("Default Aim Line")]
	
	public bool IsAimed => true;

	public void AimTarget(GameObject _target, Vector3 predictedPosition, TowerDataSO currentTower)
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
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * currentTower.rotationSpeed);
		}
	}

	public float CalculateFlightTime(Vector3 start, Vector3 target, float speed)
	{
		return 0;
	}

	public Vector3 GetPredictedPosition(GameObject target, Vector3 towerPosition, float projectileSpeed)
	{
		return (target.transform.position);
	}
}
