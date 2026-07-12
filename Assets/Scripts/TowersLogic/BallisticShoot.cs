using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallisticShoot : MonoBehaviour, IShooteable
{
    [SerializeField] public AmmoSO _currentAmmo;
    [SerializeField] public Transform _shootStartPoint;
    private GameObjectPool<CannonProjectile> _pool;

    public float CurrentProjectileSpeed
    {
        get
        {
            return _currentAmmo.ammoSpeed;
        }
    }

    public void Start()
    {
        if (_pool == null)
        {
            _pool = new GameObjectPool<CannonProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CannonTower");
        }
    }

    public void TryShoot(float fireSpeed, GameObject target)
    {
        if (RaycastCheck(target))
        {
            StartCoroutine(MakeShoot(fireSpeed, target));
        }
    }

    // private Vector3 CalculateBallisticVelocity(Vector3 start, Vector3 target, float speed)
    // {
    //     Vector3 direction = target - start;

    //     float distance = new Vector3(direction.x, 0f, direction.z).magnitude;

    //     float time = distance / speed;

    //     Vector3 velocity = direction / time;
    //     velocity.y -= 0.5f * Physics.gravity.y * time;

    //     return velocity;
    // }

    private CannonProjectile SpawnProjectile()
    {
        CannonProjectile projectile = _pool.GetObject();
        if (projectile != null)
        {
            projectile.Initialize(_currentAmmo);
            projectile.SetPool(_pool);
        }
        return projectile;
    }

    private bool RaycastCheck(GameObject target)
    {
        // Vector3 modifiedPosition = transform.position
        //    + transform.forward * 0
        //    + transform.up * 1
        //    + transform.right * 2;

        Vector3 modifiedPosition = _shootStartPoint.position;

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
        Debug.Log("Пиу");
        CannonProjectile projectile = SpawnProjectile();

        // Vector3 velocity = CalculateBallisticVelocity(_shootStartPoint.position, target.transform.position, fireSpeed);
        projectile.transform.SetPositionAndRotation(_shootStartPoint.position, _shootStartPoint.rotation);
        // projectile.rb.linearVelocity = velocity;
        projectile.rb.linearVelocity =_shootStartPoint.forward * fireSpeed;        
        
        yield return new WaitForSeconds(fireSpeed);
    }

}
