using UnityEngine;
using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.PlayerStats;

namespace AbsTowerDefense.MonsterLogic
{
	public class MonsterSpawner : MonoBehaviour
	{
		private PlayerStatsModel _model;
		private float _lastSpawn = -1;
		private float _timeLeft;
		private MonsterFactory _monsterFactory;

		[SerializeField]
		public float _interval = 3;
		[SerializeField]
		private int _maxPoolSize = 10;
		[SerializeField]
		private Collider _moveGoTo;
		[SerializeField]
		private MonsterDataSO _monsterData;
		[SerializeField]
		private GameUI gameUI;

		private void Awake()
		{
			var _monsterPool = PoolManager.Instance.CreateOrGetPool<Monster>(_monsterData.monsterPrefab, _maxPoolSize, $"{_monsterData.monsterName}");
			_monsterFactory = new MonsterFactory(_monsterPool, _monsterData);
		}

		private void Start()
		{
			_lastSpawn = -_interval;
			_model = gameUI.GetPlayerStatsModel();
		}

		private void Update()
		{
			if (Time.time > _lastSpawn + _interval)
			{
				SpawnMonster();
				_lastSpawn = Time.time;
				
			}
			_timeLeft = _lastSpawn + _interval - Time.time;
			_model.UpdateMonsterSpawnTimer(_timeLeft);
		}

		private void SpawnMonster()
		{
			var monster = _monsterFactory.CreateMonster(transform.position, _moveGoTo);
			if (monster != null)
			{
				monster.OnDied += OnMonsterDied;
			}
		}

		private void OnMonsterDied(Monster monster)
		{
			monster.OnDied -= OnMonsterDied;
			_monsterFactory.ReturnToPool(monster);
		}
	}
}