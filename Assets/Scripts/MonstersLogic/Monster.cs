using UnityEngine;
using System;
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

		private Collider _monsterGoTo;
		private float _speed = 0.1f;
		private float _hp;
		private bool _isDead;

		[SerializeField]
		private Rigidbody _rb;

		public event Action<Monster> OnDied;

		public Transform Transform => transform;
		public float Speed => _speed;
		public bool IsAlive => _hp > 0 && gameObject.activeInHierarchy;

		public void Initialize(Vector3 spawnPosition, Collider monsterGoTo, MonsterDataSO monsterData)
		{
			transform.position = spawnPosition;
			_monsterGoTo = monsterGoTo;
			_speed = monsterData.speed;
			_hp = monsterData.maxHP;
			_rb.useGravity = false;
		}

		public Vector3 MoveDirection
		{
			get
			{
				if (_monsterGoTo == null)
				{
					return Vector3.zero;
				}
				return (_monsterGoTo.transform.position - transform.position).normalized;
			}
		}

		public void OnSpawn()
		{
			_isDead = false;
		}

		public void OnDespawn()
		{
			_monsterGoTo = null;
			_isDead = true;
		}

		private void Update()
		{
			if (_monsterGoTo == null)
			{
				return;
			}
			PawnMove();
		}

		private void PawnMove()
		{
			transform.position = Vector3.MoveTowards(transform.position, _monsterGoTo.transform.position, Time.deltaTime * _speed);
		}

		private void TakeDamage(float damage)
		{
			_hp -= damage;
			if (_hp <= 0f)
			{
				_hp = 0f;
				OnAnyMonsterDeath?.Invoke();
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
			if (other.CompareTag("SystemEnemyCleaner"))
			{
				Die();
			}
			if (other.CompareTag("Projectile"))
			{
				BaseProjectile currentProjectile = other.GetComponent<BaseProjectile>();
				if (currentProjectile != null)
				{
					TakeDamage(currentProjectile.damage);
					currentProjectile.OnDespawn();	
				}
			}
		}
	}
}