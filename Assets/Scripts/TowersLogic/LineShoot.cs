using System.Collections;
using UnityEngine;

public class LineShoot : MonoBehaviour, IShooteable
{   
    [SerializeField]
    public AmmoSO _currentAmmo;
    private bool _canShoot = true;
    private GameObjectPool<GuidedProjectile> _pool;

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
            _pool = new GameObjectPool<GuidedProjectile>(_currentAmmo.ammoProjectilePrefab, 5, "CrystalTower");
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
        Instantiate(_currentAmmo.ammoProjectilePrefab, transform.position, transform.rotation); //заменить на пул
        var projectileBeh = _currentAmmo.ammoProjectilePrefab.GetComponent<GuidedProjectile> ();
        projectileBeh.Initialize(_currentAmmo, target.gameObject);
        yield return new WaitForSeconds(fireSpeed);
        _canShoot = true;
    }
}
