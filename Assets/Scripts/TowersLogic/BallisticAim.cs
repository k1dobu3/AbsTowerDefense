using System;
using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
    [SerializeField] private Transform childTransform;
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

    public void AimTarget(GameObject target, float timeToTarget, TowerDataSO currentTower)
    {
        if (currentTower.towerGunHeadMoveable) // rotate gun mounts
        {
            Rigidbody targetRB = target.GetComponent<Rigidbody>();
            Vector3 targetVelocity = targetRB != null ? targetRB.linearVelocity : Vector3.zero;
            Vector3 predictionTargetPosition = target.transform.position + (targetVelocity * timeToTarget);
            Vector3 aimDirection = predictionTargetPosition - transform.position;
            aimDirection.y = 0;

            if (aimDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * currentTower.rotationSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentTower.rotationSpeed * Time.deltaTime);
                IsBodyAimed = Quaternion.Angle(transform.rotation, targetRotation) < 2f;
                Debug.Log($"IsBodyAimed because {Quaternion.Angle(transform.rotation, targetRotation)} < then 2");
            }
        }

        if (true) //incile gun barrel
        {
            float targetBarrelAngle = float.NaN;
            //precalc
            float dx = target.transform.position.x - transform.position.x;
            float dz = target.transform.position.z - transform.position.z;
            float targetY = target.transform.position.y - transform.position.y;
            float targetX = (float)Mathf.Sqrt(dx * dx + dz * dz);


            float v2 = currentTower.startMuzzleSpeed * currentTower.startMuzzleSpeed;
            float v4 = v2 * v2;
            float rootTerm = v4 - _gravity * (_gravity * targetX * targetX + 2 * targetY * v2);

            if (rootTerm >= -25f)
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
                    if (angleDeg > -20f && angleDeg <= 60f)
                    {
                        targetBarrelAngle = Mathf.Clamp(angleDeg, -20f, 60f);
                    }

                }
            }

            if (!float.IsNaN(targetBarrelAngle))
            {
                float currentAngle = -childTransform.localEulerAngles.x;
                //float smoothedAngle = Mathf.LerpAngle(currentAngle, targetBarrelAngle, Time.deltaTime * currentTower.rotationSpeed * 1.8f);
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
        float root = v4 - _gravity * (_gravity * horizontalDistance * horizontalDistance + 2 * height *v2);

        if (root < 0)
        {
            return -1;
        }

        float angle = Mathf.Atan((v2 - Mathf.Sqrt(root))/(_gravity * horizontalDistance));
        float time = horizontalDistance / (speed * Mathf.Cos(angle));

        return time;
    }
}
