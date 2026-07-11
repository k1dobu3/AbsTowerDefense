using UnityEngine;

public class BallisticAim : MonoBehaviour, IAim
{
    public void AimTarget(bool rotateable, Transform _target,  float timeToTarget, float gunRotationSpeed)
    {
        if (rotateable) // rotate gun mounts
        {
            Rigidbody targetRB = _target.GetComponent<Rigidbody>();
            Vector3 targetVelocity = targetRB != null ? targetRB.linearVelocity : Vector3.zero;
            Vector3 predictionTargetPosition = _target.position + (targetVelocity * timeToTarget);
            Vector3 aimDirection = predictionTargetPosition - transform.position;
            aimDirection.y = 0;

            if (aimDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * gunRotationSpeed);
            }
        }

        if (true) //incile gun barrel
        {
            //Debug.Log("Ствол наклоняется");
        }
    }

    public void AimReset(bool rotateable, Vector3 defaultRotationEuler, float gunRotationSpeed)
    {
        if (rotateable)
        {
            Quaternion defaultRot = Quaternion.Euler(defaultRotationEuler);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRot, Time.deltaTime * gunRotationSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(defaultRotationEuler), Time.deltaTime * gunRotationSpeed);
        }
    }
}
