using UnityEngine;
using System;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.GameObjectPool.Abstract;
using AbsTowerDefense.TowersLogic;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.MonsterLogic
{
	public class Monster : MonoBehaviour, IPoolable, IDamageable
	{
		public static event Action OnAnyMonsterDeath;

		public float speed { get {return _speed;} set {_speed = value;} }
		public float hp { get {return _hp;} set {_hp = value;} }
		// public Rigidbody rb { get {return _rb;}}

		private GameObject _moveTarget;
		private float _speed = 0.1f;
		private float _hp;
		private float _reachDistance = 0.5f;
		private bool _isDead;

		[SerializeField]
		private Rigidbody _rb;

		public event Action<Monster> OnDied;

		public Transform Transform => transform;
		public float Speed => _speed;
		public bool IsAlive => _hp > 0 && gameObject.activeInHierarchy;

		public void Initialize(Vector3 spawnPosition, Transform moveTarget, MonsterDataSO monsterData)
		{
			transform.position = spawnPosition;
			_moveTarget = moveTarget.gameObject;
			_speed = monsterData.speed;
			_hp = monsterData.maxHP;
			_rb.useGravity = false;
		}

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

		public void OnSpawn()
		{
			_isDead = false;
		}

		public void OnDespawn()
		{
			_moveTarget = null;
			_isDead = true;
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
				Die();
			}
		}

		private void Die()
		{
			if (_isDead)
			{
				return;
			}
			OnDespawn();
			OnDied?.Invoke(this);
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
}