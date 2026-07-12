using System;
using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
    [SerializeField] private Transform childTransform;
    private float _gravity;

    private void Start()
    {
        _gravity = SceneRule.Instance.SceneGravity;
    }

    public void AimTarget(bool rotateable, GameObject target, float timeToTarget, TowerDataSO currentTower)
    {
        if (rotateable) // rotate gun mounts
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
            float resultBarrelAngle = float.MaxValue;
            //precalc
            float dx = target.transform.position.x - transform.position.x;
            float dz = target.transform.position.z - transform.position.z;
            float targetY = target.transform.position.y - transform.position.y;
            float targetX = (float)Mathf.Sqrt(dx * dx + dz * dz);


            float v2 = currentTower.startMuzzleSpeed * currentTower.startMuzzleSpeed;
            float v4 = v2 * v2;
            float rootTerm = v4 - _gravity * (_gravity * targetX * targetX + 2 * targetY * v2);

            if (rootTerm < 0)
            {
                resultBarrelAngle = float.NaN;
            }

            float numerator = v2 - (float)Math.Sqrt(rootTerm);
            float denominator = _gravity * targetX;

            if (Mathf.Abs(denominator) < 0.0001f)
            {
                resultBarrelAngle = float.NaN;
            }

            float angleRad = (float)Math.Atan(numerator / denominator);
            float angleDeg = angleRad * (180f / (float)Math.PI);

            if (angleRad < 0)
            {
                angleRad = (float)Math.PI + angleRad;
            }

            if (angleDeg < 0 || angleDeg > 90f)
            {
                resultBarrelAngle = float.NaN;
            }

            resultBarrelAngle = angleRad * (180f / (float)Math.PI);
            Debug.Log(resultBarrelAngle);

            if (resultBarrelAngle >= 0f)
            {
                childTransform.localEulerAngles = new Vector3(-resultBarrelAngle, 0, 0);
            }
        }
    }

    public void AimReset(bool rotateable, Vector3 defaultRotationEuler, TowerDataSO currentTower)
    {
        if (rotateable)
        {
            Quaternion defaultRot = Quaternion.Euler(defaultRotationEuler);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRot, Time.deltaTime * currentTower.rotationSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * gunRotationSpeed);
        }
        if (true)
        {
            Quaternion defaultRot = Quaternion.Euler(0,0,0);
            childTransform.localRotation = Quaternion.Slerp(childTransform.localRotation, defaultRot, currentTower.barrelRotationSpeed * Time.deltaTime);
        }
    }
}
