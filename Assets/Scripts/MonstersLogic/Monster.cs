using UnityEngine;
using System;
using Unity.VisualScripting;

public class Monster : MonoBehaviour, IPoolable, IDamageable
{
	public static event Action OnAnyMonsterDeath;

	public float speed { get {return _speed;} set {_speed = value;} }
	public float hp { get {return _hp;} set {_hp = value;} }
	public Rigidbody rb { get {return _rb;}}

	private GameObject _moveTarget;
	private float _speed = 0.1f;
	private float _hp;
	private float _reachDistance = 0.5f;
	private GameObjectPool<Monster> _pool;

	[SerializeField]
	private Rigidbody _rb;

	public Vector3 MoveDirection
	{
		get
		{
			if (_moveTarget == null)
			{
				return Vector3.zero;
			}
			return (_moveTarget.transform.position - transform.position).normalized;
		}
	}

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

	public bool IsDead()
	{
		return _hp <= 0;
	}

	private void Update()
	{
		if (_moveTarget == null)
			return;

		if (Vector3.Distance(transform.position, _moveTarget.transform.position) <= _reachDistance)
		{
			OnDespawn();
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

	private void TakeDamage(float damage, bool systemKill)
	{
		_hp -= damage;
		if (_hp <= 0f)
		{
			_hp = 0f;
			if (!systemKill)
			{
				OnAnyMonsterDeath?.Invoke();
			}
			OnDespawn();
			IsDead();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Projectile"))
		{
			BaseProjectile currentProjectile = other.GetComponent<BaseProjectile>();
			if (currentProjectile != null)
			{
				TakeDamage(currentProjectile.damage, false);
				currentProjectile.OnDespawn();	
			}
		}
	}
}
