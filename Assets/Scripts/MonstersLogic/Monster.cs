using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;

public class Monster : MonoBehaviour, IPoolable, IDamageable 
{
	public GameObject _moveTarget;
	private float _speed = 0.1f;
	private float _hp;
	const float _reachDistance = 0.5f;
	private GameObjectPool<Monster> _pool;
	public static event Action OnAnyMonsterDeath;

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
			TakeDamage(_hp, true);
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

	public void TakeDamage(float damage, bool systemKill) 
	{
		_hp -= damage;
		if (_hp <= 0f) {
			_hp = 0f;
			if (!systemKill)
			{
                OnAnyMonsterDeath?.Invoke();
			}
			OnDespawn();
			IsDead();
		}
	}

	public bool IsDead()
	{
		return _hp <= 0;
	}

    private void OnDestroy()
    {
        OnAnyMonsterDeath = null;
    }
}
