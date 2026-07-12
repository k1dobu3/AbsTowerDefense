using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallisticAim : MonoBehaviour, IAim
{
    [SerializeField] private Transform childTransform;
    [SerializeField] public Transform _shootStartPoint;
    [Header("Default Aim Line")]
    [SerializeField] private Vector3 defaultRotationEuler = new Vector3(0, 0, 0);
    public bool IsBodyAimed { get; private set; }
    public bool IsBarrelAimed { get; private set; }
    public bool IsAimed => IsBodyAimed && IsBarrelAimed;
    private float _gravity;

    private void Start()
    {
        _gravity = SceneRule.Instance.SceneGravity;
    }

    public void AimTarget(GameObject target, Vector3 predictedPosition, TowerDataSO currentTower)
    {
        if (currentTower.towerGunHeadMoveable) // rotate gun mounts
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

        if (true) //incile gun barrel
        {
            float targetBarrelAngle = float.NaN;
            //precalc
            float dx = predictedPosition.x - transform.position.x;
            float dz = predictedPosition.z - transform.position.z;
            float targetY = predictedPosition.y - transform.position.y;

            float targetX = (float)Mathf.Sqrt(dx * dx + dz * dz);


            float v2 = currentTower.startMuzzleSpeed * currentTower.startMuzzleSpeed;
            float v4 = v2 * v2;
            float rootTerm = v4 - _gravity * (_gravity * targetX * targetX + 2 * targetY * v2);

            if (rootTerm >= -40f)
            {
                float numerator = v2 - Mathf.Sqrt(rootTerm);
                float denominator = _gravity * targetX;

                if (Mathf.Abs(denominator) > 0.0001f)
                {
                    float angleRad = Mathf.Atan(numerator / denominator);

                    if (angleRad < 0)
                    {
                        angleRad = Mathf.PI + angleRad;
                    }
                    float angleDeg = angleRad * (180f / (float)Math.PI);
                    if (angleDeg > -40f && angleDeg <= 90f)
                    {
                        targetBarrelAngle = Mathf.Clamp(angleDeg, -40f, 90f);
                    }

                }
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
    }

    public void AimReset(TowerDataSO currentTower)
    {
        IsBodyAimed = false;
        IsBarrelAimed = false;
        if (currentTower.towerGunHeadMoveable)
        {
            Quaternion defaultRot = Quaternion.Euler(defaultRotationEuler);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRot, Time.deltaTime * currentTower.rotationSpeed);
        }
        if (true)
        {
            Quaternion defaultBarrelRot = Quaternion.Euler(defaultRotationEuler);
            childTransform.localRotation = Quaternion.RotateTowards(childTransform.localRotation, defaultBarrelRot, Time.deltaTime * currentTower.barrelRotationSpeed);
        }
    }

    public float CalculateFlightTime(Vector3 start, Vector3 target, float speed)
    {
        Vector3 direction = target - start;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;

        float height = direction.y;
        float v2 = speed * speed;
        float v4 = v2 * v2;
        float root = v4 - _gravity * (_gravity * horizontalDistance * horizontalDistance + 2 * height * v2);

        if (root < 0)
        {
            return -1;
        }

        float angle = Mathf.Atan((v2 - Mathf.Sqrt(root)) / (_gravity * horizontalDistance));
        float time = horizontalDistance / (speed * Mathf.Cos(angle));

        return time;
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

        Vector3 aimPoint = currentPosition;
        aimPoint.y += 1f;

        float time = Vector3.Distance(_shootStartPoint.position, currentPosition) / projectileSpeed;
        Vector3 predictedPos = currentPosition + targetVelocity * time;
        for (int i = 0; i < 6; i++)
        {
            predictedPos = currentPosition + targetVelocity * time;
            predictedPos.y += 2f;

            float newTime = CalculateFlightTime(_shootStartPoint.position, predictedPos, projectileSpeed);
            if (newTime < 0)
                return predictedPos;

            time = newTime;
        }
        return predictedPos;
    }
}
