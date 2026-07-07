using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
    public void AimTarget(bool rotateable, Transform _target,  float timeToTarget, float gunRotationSpeed)
    {
        if (rotateable)
        {
            // Vector3 direction = _target.position - transform.position;
            // direction.y = 0;

            Rigidbody targetRB = _target.GetComponent<Rigidbody>();
            Vector3 targetVelocity = targetRB != null ? targetRB.linearVelocity : Vector3.zero;
            Vector3 predictionTargetPosition = _target.position + (targetVelocity * timeToTarget);
            Vector3 aimDirection = predictionTargetPosition - transform.position;
            aimDirection.y = 0;

            if (aimDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * gunRotationSpeed);
                // float targetY = targetRotation.eulerAngles.y;
                // float currentY = transform.rotation.eulerAngles.y;
                // float newY = Mathf.LerpAngle(currentY, targetY, Time.deltaTime * _currentTower.rotationSpeed);

                // transform.rotation = Quaternion.Euler(0, newY, 0);
            }
        }
    }

        public void AimReset(bool rotateable, Vector3 defaultRotationEuler, float gunRotationSpeed)
    {
        if (rotateable)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * gunRotationSpeed);
        }
    }
}
