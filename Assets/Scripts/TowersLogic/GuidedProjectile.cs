using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour, IPoolable
{
	private GameObject _target;
	private AmmoSO _currentAmmoData;
	private GameObjectPool<GuidedProjectile> _pool;
	private Collider _collider;

	public void Initialize(AmmoSO ammoData, GameObject target)
	{
		_currentAmmoData = ammoData;
		_target = target;
	}
	
	public void SetPool(GameObjectPool<GuidedProjectile> pool) 
	{
		_pool = pool;
	}

	public void OnSpawn()
	{
		if (_collider == null)
		{
			_collider = GetComponent<Collider>();			
		}
	}

    public void OnDespawn()
    {
		_target = null;
		_pool.ReturnObject(this);
    }

	private void Update () {
		if (_target == null || !_target.gameObject.activeInHierarchy) {
			OnDespawn();
			return;
		}

		var translation = _target.transform.position - transform.position;
		if (translation.magnitude > _currentAmmoData.ammoSpeed) {
			translation = translation.normalized * _currentAmmoData.ammoSpeed;
		}
		transform.Translate (translation);
	}

	private void OnTriggerEnter(Collider other) 
	{
		_collider.enabled = false;
		var monster = other.gameObject.GetComponent<IDamageable>();
		if (monster == null)
		{
			return;	
		}
		monster.TakeDamage (_currentAmmoData.ammoDamage, false);
		OnDespawn();
	}
}
