using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CannonProjectile : MonoBehaviour, IPoolable
{
	private GameObjectPool<CannonProjectile> _pool;
	private AmmoSO _currentAmmoData;
	private float _spawnTime;
	public Rigidbody rb;

	private void Awake()
	{
    	rb = GetComponent<Rigidbody>();
	}
	public void Initialize(AmmoSO ammoData)
	{
		_currentAmmoData = ammoData;
	}

	public void SetPool(GameObjectPool<CannonProjectile> pool) 
	{
		_pool = pool;
	}

    public void FixedUpdate()
    {
        if (Time.time - _spawnTime > 5f)
		{
			OnDespawn();
		}
    }

    public void OnSpawn()
	{
		rb.linearVelocity = Vector3.zero;
    	rb.angularVelocity = Vector3.zero;
		_spawnTime = Time.time;
	}

	public void OnDespawn()
	{
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		_pool.ReturnObject(this);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.TakeDamage (_currentAmmoData.ammoDamage, false);
		OnDespawn();
	}
}
