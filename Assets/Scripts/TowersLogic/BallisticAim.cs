using UnityEngine;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic;
using AbsTowerDefense.Rules;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
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

		public void AimTarget(IDamageable target, Vector3 predictedPosition, TowerDataSO currentTower)
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
			if (!float.IsNaN(angleDeg) && angleDeg > 40f && angleDeg <= 89f)
			{
				angleDeg *= 1.08f;
				targetBarrelAngle = Mathf.Clamp(angleDeg, 40f, 89f);
				Debug.Log(targetBarrelAngle);
			}

			if (!float.IsNaN(targetBarrelAngle))
			{
				float currentAngle = -childTransform.localEulerAngles.x;
				float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, targetBarrelAngle, currentTower.barrelRotationSpeed * Time.deltaTime);
				childTransform.localEulerAngles = new Vector3(-smoothedAngle, 0, 0);
				IsBarrelAimed = Mathf.Abs(Mathf.DeltaAngle(smoothedAngle, targetBarrelAngle)) < 3f;
				Debug.Log($"IsBarrelAimed because {Mathf.Abs(Mathf.DeltaAngle(smoothedAngle, targetBarrelAngle))} < then 3");
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
			Vector3 to = target - start;
			float horizontal = new Vector2(to.x, to.z).magnitude;
			float height = to.y;

			float angle = CalcShootAngle(horizontal, height, speed);
			if (float.IsNaN(angle))
			{
				return -1f;
			}

			return angle;
		}
		
		public float CalcFlightTime(float horizontalDistance, float angleDeg, float speed)
		{
			float angleRad = angleDeg * Mathf.Deg2Rad;
			float cos = Mathf.Cos(angleRad);
			if (Mathf.Abs(cos) < 0.0001f)
			{
				return float.NaN;
			}
			return horizontalDistance / (speed * cos);
		}

		public Vector3 GetPredictedPosition(IDamageable target, Vector3 towerPosition, float projectileSpeed)
		{
			Monster monster = target as Monster;
			if (monster == null)
				return target.Transform.position;

			Vector3 targetVel = monster.MoveDirection * monster.speed;
			Vector3 currentTargetPos = target.Transform.position;
			Vector3 aimOffset = new Vector3(0f, 2.3f, 0f);
			Vector3 startPos = _shootStartPoint.position;

			Vector3 predicted = currentTargetPos + aimOffset;
			const int iterations = 10;

			for (int i = 0; i < iterations; i++)
			{
				Vector3 to = predicted - startPos;
				float distXZ = new Vector2(to.x, to.z).magnitude;
				float height = to.y;

				float angle = CalcShootAngle(distXZ, height, projectileSpeed);
				if (float.IsNaN(angle))
				{
					break;					
				}

				float flightT = CalcFlightTime(distXZ, angle, projectileSpeed);
				if (float.IsNaN(flightT) || flightT <= 0f)
				{
					break;					
				}

				float timeFlightScale = 1f;
				Vector3 newPredicted = currentTargetPos + targetVel * (flightT * timeFlightScale) + aimOffset;

				if (Vector3.Distance(newPredicted, predicted) < 0.05f)
				{
					break;					
				}

				predicted = newPredicted;
			}

			return predicted;
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
			if (forceReserve < 10f)
			{
				return float.NaN;
			}

			float numerator = v2 + Mathf.Sqrt(forceReserve);
			float denominator = _gravity * horizontalDistance;
			if (Mathf.Abs(denominator) < 0.0001f)
			{
				return float.NaN;
			}
			float angleRad = Mathf.Atan(numerator / denominator);
			if (angleRad < 0)
			{
				angleRad += Mathf.PI;
			}
			float angleDeg = angleRad * Mathf.Rad2Deg;

			return angleDeg;
		}
	}
}