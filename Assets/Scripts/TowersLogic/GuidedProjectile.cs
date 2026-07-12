using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour, IPoolable
{
	private GameObject _target;
	private AmmoSO _currentAmmoData;
	private GameObjectPool<GuidedProjectile> _pool;

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
        
    }

    public void OnDespawn()
    {
		_target = null;
		_pool.ReturnObject(this);
    }


	void Update () {
		if (_target == null || !_target.gameObject.activeInHierarchy) {
			Destroy (gameObject);
			Debug.Log("Полетел");
			return;
		}

		var translation = _target.transform.position - transform.position;
		if (translation.magnitude > _currentAmmoData.ammoSpeed) {
			translation = translation.normalized * _currentAmmoData.ammoSpeed;
		}
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {

		GetComponent<Collider>().enabled = false;

		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.TakeDamage (_currentAmmoData.ammoDamage, false);
		_pool.ReturnObject(this);
	}
}
