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
		private bool _canAimBarrel = false;
		private const float forceReserveThreshold = 10f;

		[Header("Default Tower Rotation Settings")]
		[SerializeField]
		private float _defaultGunHeadAngle = 0f;
		
		[Header("Default Aim Line")]
		[SerializeField]
		private Transform childTransform;
		[SerializeField]
		private Transform _shootStartPoint;

		[Header("Aim settings for position")]
		[SerializeField]
		private float angleScale = 1f;
		[SerializeField]
		private float targetAimHighOffset = 2.3f;
		[SerializeField]
		private float timeFlightScale = 1.0f;
		[SerializeField]
		private float gunHeadAimTolerance = 2f;
		[SerializeField]
		private float barrelAimTolerance = 3f;
		[SerializeField]
		private float predictionDistence = 0.5f;

		public bool IsAimed => IsBodyAimed && IsBarrelAimed;

		public void AimTarget(IDamageable target, Vector3 predictedPosition, TowerDataSO currentTower)
		{
			if (currentTower.towerGunHeadMoveable)
			{
				Vector3 aimDirection = predictedPosition - _shootStartPoint.position;
				aimDirection.y = 0;

				if (aimDirection != Vector3.zero)
				{
					Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
					transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentTower.rotationSpeed * Time.deltaTime);
					IsBodyAimed = Quaternion.Angle(transform.rotation, targetRotation) < gunHeadAimTolerance;
				}
			}

			float targetBarrelAngle = 0f;

			float dx = predictedPosition.x - _shootStartPoint.position.x;
			float dz = predictedPosition.z - _shootStartPoint.position.z;
			float targetY = predictedPosition.y - _shootStartPoint.position.y;
			float targetX = Mathf.Sqrt(dx * dx + dz * dz);
			
			float angleDeg = CalcShootAngle(targetX, targetY, currentTower.startMuzzleSpeed);
			if (!float.IsNaN(angleDeg) && angleDeg > 35f && angleDeg <= 89f)
			{
				angleDeg *= angleScale;
				targetBarrelAngle = Mathf.Clamp(angleDeg, 35f, 89f);
				_canAimBarrel = true;
			}
			else
			{
				_canAimBarrel = false;
			}

			if (_canAimBarrel)
			{
				float currentAngle = -childTransform.localEulerAngles.x;
				float smoothedAngle = Mathf.MoveTowardsAngle(currentAngle, targetBarrelAngle, currentTower.barrelRotationSpeed * Time.deltaTime);
				childTransform.localEulerAngles = new Vector3(-smoothedAngle, 0, 0);
				IsBarrelAimed = Mathf.Abs(Mathf.DeltaAngle(smoothedAngle, targetBarrelAngle)) < barrelAimTolerance;
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
				Quaternion defaultRot = Quaternion.Euler(new Vector3(0f, _defaultGunHeadAngle, 0f));
				transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRot, Time.deltaTime * currentTower.rotationSpeed);
			}
			Quaternion defaultBarrelRot = Quaternion.Euler(Vector3.zero);
			childTransform.localRotation = Quaternion.RotateTowards(childTransform.localRotation, defaultBarrelRot, Time.deltaTime * currentTower.barrelRotationSpeed);
		}

		public float CalculateFlightTime(float horizontalDistance, float angleDeg, float speed)
		{
			float angleRad = angleDeg * Mathf.Deg2Rad;
			float angleCos = Mathf.Cos(angleRad);
			if (Mathf.Abs(angleCos) < 0.0001f)
			{
				return float.NaN;
			}
			return horizontalDistance / (speed * angleCos);
		}

		public Vector3 GetPredictedPosition(IDamageable target, Vector3 towerPosition, float projectileSpeed)
		{
			Monster monster = target as Monster;
			if (monster == null)
			{
				return target.Transform.position;				
			}

			Vector3 targetVelocity = monster.MoveDirection * monster.speed;
			Vector3 currentTargetPos = target.Transform.position;
			Vector3 aimOffset = new Vector3(0f, targetAimHighOffset, 0f);
			Vector3 shootStartPos = _shootStartPoint.position;

			Vector3 predicted = currentTargetPos + aimOffset;
			const int iterations = 10;

			for (int i = 0; i < iterations; i++)
			{
				Vector3 to = predicted - shootStartPos;
				float distXZ = new Vector2(to.x, to.z).magnitude;
				float height = to.y;

				float angleDeg = CalcShootAngle(distXZ, height, projectileSpeed);
				if (float.IsNaN(angleDeg))
				{
					break;					
				}

				float flightT = CalculateFlightTime(distXZ, angleDeg, projectileSpeed);
				if (float.IsNaN(flightT) || flightT <= 0f)
				{
					break;					
				}

				Vector3 newPredicted = currentTargetPos + targetVelocity * (flightT * timeFlightScale) + aimOffset;

				if (Vector3.Distance(newPredicted, predicted) < predictionDistence)
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
			if (forceReserve < forceReserveThreshold)
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