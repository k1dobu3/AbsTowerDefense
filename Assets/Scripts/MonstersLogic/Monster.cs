using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour, IPoolable, IDamageable 
{
	public GameObject _moveTarget;
	private float _speed = 0.1f;
	private float _hp = 100;
	const float _reachDistance = 0.5f;
	private GameObjectPool<Monster> _pool;

	public float speed { get { return _speed; } set { _speed = value; } }
	public float hp { get { return _hp; } set { _hp = value; } }

	public void SetPool(GameObjectPool<Monster> pool) 
	{
		_pool = pool;
	}
	
	public void OnSpawn()
	{
	}
	
	public void OnDespawn() 
	{
		_moveTarget = null;
		_pool.ReturnObject(this);

	}

	public void SetMoveTarget(GameObject target) 
	{
		_moveTarget = target;
	}

	void Update () {
		if (_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, _moveTarget.transform.position) <= _reachDistance) {
			TakeDamage(_hp);
			return;
		}
		else
		{
			PawnMove();
		}
	}

	private void PawnMove()
	{
		transform.position = Vector3.MoveTowards(transform.position, _moveTarget.transform.position, Time.deltaTime * _speed);
	}

	public void TakeDamage(float damage) 
	{
		_hp -= damage;
		if (_hp <= 0f) {
			_hp = 0f;
			OnDespawn();
			IsDead();
		}
	}

	public bool IsDead()
	{
		return _hp <= 0;
	}
}
