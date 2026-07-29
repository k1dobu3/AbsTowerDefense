using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
	public bool IsBodyAimed { get; private set; }
	public bool IsBarrelAimed { get; private set; }

	private float _gravity;

	[SerializeField]
	private Transform childTransform;
	[SerializeField]
	private Transform _shootStartPoint;
	[Header("Default Aim Line")]

	public bool IsAimed => IsBodyAimed && IsBarrelAimed;

	public void AimTarget(GameObject target, Vector3 predictedPosition, TowerDataSO currentTower)
	{
		if (currentTower.towerGunHeadMoveable)
		{
			Vector3 aimDirection = predictedPosition - transform.position;
			aimDirection.y = 0;

			if (aimDirection != Vector3.zero)
			{
				Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentTower.rotationSpeed * Time.deltaTime);
				IsBodyAimed = Quaternion.Angle(transform.rotation, targetRotation) < 2f;
				Debug.Log($"IsBodyAimed because {Quaternion.Angle(transform.rotation, targetRotation)} < then 2");
			}
		}

		float targetBarrelAngle = 0f;

		float dx = predictedPosition.x - transform.position.x;
		float dz = predictedPosition.z - transform.position.z;
		float targetY = predictedPosition.y - transform.position.y;
		float targetX = (float)Mathf.Sqrt(dx * dx + dz * dz);
		
		float angleDeg = CalcShootAngle(targetX, targetY, currentTower.startMuzzleSpeed);
		if (!float.IsNaN(angleDeg) && angleDeg > -60f && angleDeg <= 85f)
		{
			targetBarrelAngle = Mathf.Clamp(angleDeg, -60f, 85f);
			Debug.Log(targetBarrelAngle);
		}

		if (!float.IsNaN(targetBarrelAngle))
		{
			float currentAngle = -childTransform.localEulerAngles.x;
			float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, targetBarrelAngle, currentTower.barrelRotationSpeed * Time.deltaTime);
			childTransform.localEulerAngles = new Vector3(-smoothedAngle, 0, 0);
			IsBarrelAimed = Mathf.Abs(Mathf.DeltaAngle(smoothedAngle, targetBarrelAngle)) < 3f;
			Debug.Log($"IsBarrelAimed because {Mathf.Abs(Mathf.DeltaAngle(smoothedAngle, targetBarrelAngle))} < then 1");
		}
		else
		{
			AimReset(currentTower);
		}
	}

	public void AimReset(TowerDataSO currentTower)
	{
		IsBodyAimed = false;
		IsBarrelAimed = false;
		if (currentTower.towerGunHeadMoveable)
		{
			Quaternion defaultRot = Quaternion.Euler(Vector3.zero);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRot, Time.deltaTime * currentTower.rotationSpeed);
		}
		Quaternion defaultBarrelRot = Quaternion.Euler(Vector3.zero);
		childTransform.localRotation = Quaternion.RotateTowards(childTransform.localRotation, defaultBarrelRot, Time.deltaTime * currentTower.barrelRotationSpeed);
	}

	public float CalculateFlightTime(Vector3 start, Vector3 target, float speed)
	{
		float directDist = Vector3.Distance(start, target);
		if (speed <= 0f)
		{
			return -1f;
		}
		float directTime = directDist / speed;

		float heightDiff = target.y - start.y;
		float extraTimeFactor = 1.0f + Mathf.Abs(heightDiff) * 0.015f;

		return directTime * extraTimeFactor;
	}

	public Vector3 GetPredictedPosition(GameObject target, Vector3 towerPosition, float projectileSpeed)
	{
		Monster monster = target.GetComponent<Monster>();
		if (monster == null)
		{
			return target.transform.position;
		}
		Vector3 moveDirection = monster.MoveDirection;
		Vector3 targetVelocity = moveDirection * monster.speed;
		Vector3 currentPosition = target.transform.position;

		Vector3 aimOffset = new Vector3(0, 2.3f, 0);
		Vector3 aimStart = currentPosition + aimOffset;

		float time = Vector3.Distance(_shootStartPoint.position, currentPosition) / projectileSpeed;
		Vector3 predictedPos = aimStart + targetVelocity * time;
		for (int i = 0; i < 12; i++)
		{
			float newTime = CalculateFlightTime(_shootStartPoint.position, predictedPos, projectileSpeed);
			time = newTime;
			if (newTime < 0)
			{
				break;
			}
			Vector3 newPredictedPos = currentPosition + targetVelocity * time + aimOffset;
			if (Vector3.Distance(newPredictedPos, predictedPos) < 0.01f)
			{
				break;
			}

			predictedPos = newPredictedPos;
			
		}
		return predictedPos;
	}

	private void Start()
	{
		_gravity = SceneRule.Instance.sceneGravity;
	}

	private float CalcShootAngle(float horizontalDistance, float heightDifference, float speed)
	{
		if (Mathf.Abs(horizontalDistance) < 0.0001f)
		{
			return float.NaN;
		}
		float v2 = speed * speed;
		float v4 = v2 * v2;
		float forceReserve = v4 - _gravity * (_gravity * horizontalDistance * horizontalDistance + 2 * heightDifference * v2);
		if (forceReserve < -10f)
		{
			return float.NaN;
		}

		float numerator = v2 - Mathf.Sqrt(forceReserve);
		float denominator = _gravity * horizontalDistance;
		if (Mathf.Abs(denominator) < 0.0001f)
		{
			return float.NaN;
		}
		float angleRad = Mathf.Atan(numerator / denominator);
		if (angleRad < 0)
		{
			angleRad = Mathf.PI + angleRad;
		}
		float angleDeg = angleRad * Mathf.Rad2Deg;

		return angleDeg;
	}
}
