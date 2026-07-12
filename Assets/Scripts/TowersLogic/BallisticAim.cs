using System;
using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
    [SerializeField] private Transform childTransform;
    [Header("Default Aim Line")]
    [SerializeField] private Vector3 defaultRotationEuler = new Vector3(0, 0, 0);
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
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * currentTower.rotationSpeed);
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
                    if (angleDeg > -20f || angleDeg <= 60f)
                    {
                        targetBarrelAngle = Mathf.Clamp(angleDeg, -20f, 60f);
                    }

                }
            }

            if (!float.IsNaN(targetBarrelAngle))
            {
                float currentAngle = -childTransform.localEulerAngles.x;
                float smoothedAngle = Mathf.LerpAngle(currentAngle, targetBarrelAngle, Time.deltaTime * currentTower.rotationSpeed * 1.8f);

                childTransform.localEulerAngles = new Vector3(-smoothedAngle, 0, 0);
            }
            else
            {
                AimReset(currentTower);
            }
        }
    }

    public void AimReset(TowerDataSO currentTower)
    {
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
}
