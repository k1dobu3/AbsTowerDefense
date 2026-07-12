using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallisticShoot : MonoBehaviour, IShooteable
{
    [SerializeField]
    public AmmoSO _currentAmmo;
    private bool _canShoot = true;

    public float CurrentProjectileSpeed
    {
        get
        {
            return _currentAmmo.ammoSpeed;
        }
    }

    public void TryShoot(float fireSpeed, GameObject target)
    {
        if (_canShoot && RaycastCheck(target))
        {
            StartCoroutine(MakeShoot(fireSpeed, target));
        }
    }

    private bool RaycastCheck(GameObject target)
    {
        Vector3 modifiedPosition = transform.position
           + transform.forward * 0
           + transform.up * 1
           + transform.right * 2;

            Ray ray = new Ray(modifiedPosition, (target.transform.position - modifiedPosition).normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red, 5.0f);
                Debug.Log("Прицел в: " + hit.collider.name);
                //Debug.LogError("EX");  //for debug pause
                if (target != null && hit.collider.CompareTag("Monster"))
                {
                    return true;
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 5.0f);
                return false;
            }
            return false;
    }

    private IEnumerator MakeShoot(float fireSpeed, GameObject target)
    {
        _canShoot = false;
        Debug.Log("Пиу");
        yield return new WaitForSeconds(fireSpeed);
        _canShoot = true;
    }

}
