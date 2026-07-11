using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour, IPoolable
{
	public float _speed = 20f;

	private float _cannonDamage;
	private GameObjectPool<CannonProjectile> _pool;

	public float cannonDamage { get {return _cannonDamage;} set {_cannonDamage = value;} }

	public void OnSpawn()
	{
	}

	public void OnDespawn()
	{
		_pool.ReturnObject(this);
	}


	void Update () {
		var translation = transform.forward * _speed;
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.TakeDamage (_cannonDamage, false);
		OnDespawn();
	}
}
