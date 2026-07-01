using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Spawner : MonoBehaviour
{
	[SerializeField] public float _interval = 3;
	[SerializeField] private float _speed = 0.2f;
	[SerializeField] private int _maxPoolSize = 10;
	[SerializeField] private GameObject m_moveTarget;
	[SerializeField] private Monster _enemyPrefab;
	private float _lastSpawn = -1;
	private GameObjectPool<Monster> _monsterPool;


	public void Awake()
	{
		_monsterPool = new GameObjectPool<Monster>(_enemyPrefab, _maxPoolSize, "Monster Pool");
	}

	public void Start()
	{
		_lastSpawn = Time.time;
	}

	void Update()
	{
		if (Time.time > _lastSpawn + _interval)
		{
			SpawnMonster();
			_lastSpawn = Time.time;
		}
	}

	public void SpawnMonster()
	{
		Monster monster = _monsterPool.GetObject();
		monster.transform.position = transform.position;
		monster.m_moveTarget = m_moveTarget;
		monster.m_speed = _speed;
		monster.SetPool(_monsterPool);

		if (monster.GetComponent<Rigidbody>() == null)
		{
			monster.AddComponent<Rigidbody>().useGravity = false;
		}
	}

	public void ReturnToPool(Monster monster)
	{
		_monsterPool.ReturnObject(monster);
	}

	private void OnDestroy()
	{
		_monsterPool.ClearPool();
	}
}
