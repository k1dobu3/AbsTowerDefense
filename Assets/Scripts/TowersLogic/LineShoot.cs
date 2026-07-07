using System.Collections;
using UnityEngine;

public class LineShoot : MonoBehaviour, IShooteable
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

    public void TryShoot(float fireSpeed, Transform target)
    {
        if (_canShoot && target != null)
        {
            StartCoroutine(MakeShoot(fireSpeed, target));
        }    
    } 


    private IEnumerator MakeShoot(float fireSpeed, Transform target)
    {
        _canShoot = false;
        Instantiate(_currentAmmo.ammoProjectilePrefab, transform.position, transform.rotation); //заменить на пул, добавит поворот к цели
        var projectileBeh = _currentAmmo.ammoProjectilePrefab.GetComponent<GuidedProjectile> ();
		projectileBeh.m_target = target.gameObject;
        yield return new WaitForSeconds(fireSpeed);
        _canShoot = true;
    }
}
