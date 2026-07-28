using UnityEngine;

public class CannonProjectile : MonoBehaviour, IPoolable
{
	private GameObjectPool<CannonProjectile> _pool;
	private AmmoSO _currentAmmoData;
	private float _spawnTime;
	private Rigidbody _rb;

	public void Initialize(AmmoSO ammoData)
	{
		_currentAmmoData = ammoData;
	}

	public void SetPool(GameObjectPool<CannonProjectile> pool) 
	{
		_pool = pool;
	}

	public void OnSpawn()
	{
		_rb.linearVelocity = Vector3.zero;
		_rb.angularVelocity = Vector3.zero;
		_spawnTime = Time.time;
	}

	public void OnDespawn()
	{
		_rb.linearVelocity = Vector3.zero;
		_rb.angularVelocity = Vector3.zero;
		_pool.ReturnObject(this);
	}

	public void SetVelocity(Vector3 Velocity)
	{
		_rb.linearVelocity = Velocity;
	}

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if (Time.time - _spawnTime > 5f)
		{
			OnDespawn();
		}
	}

	private void OnTriggerEnter(Collider other) 
	{
		var monster = other.gameObject.GetComponent<IDamageable>();
		if (monster == null)
		{
			return;	
		}
		monster.TakeDamage (_currentAmmoData.ammoDamage, false);
		OnDespawn();
	}
}
