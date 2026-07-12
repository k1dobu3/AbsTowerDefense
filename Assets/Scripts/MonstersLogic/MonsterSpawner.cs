using UnityEngine;
using Unity.VisualScripting;

public class Spawner : MonoBehaviour
{
	[SerializeField] public float _interval = 3;
	[SerializeField] private int _maxPoolSize = 10;
	[SerializeField] private GameObject _moveTarget;
	[SerializeField] private MonsterDataSO _monsterData;
	[SerializeField] private GameUI gameUI;

    private PlayerStatsModel _model;
	private float _lastSpawn = -1;
	private float _timeLeft;
	private GameObjectPool<Monster> _monsterPool;


	public void Awake()
	{
		_monsterPool = new GameObjectPool<Monster>(_monsterData.monsterPrefab, _maxPoolSize, "Monster Pool");
	}

	public void Start()
	{
		_lastSpawn = -_interval;
		_model = gameUI.GetPlayerStatsModel();
	}

	void Update()
	{
		if (Time.time > _lastSpawn + _interval)
		{
			SpawnMonster();
			_lastSpawn = Time.time;
			
		}
		_timeLeft = (_lastSpawn + _interval) - Time.time;
		_model.UpdateTimer(_timeLeft);
	}

	public void SpawnMonster()
	{
		Monster monster = _monsterPool.GetObject();
		if (monster != null)
		{
			monster.transform.position = transform.position;
			monster.SetMoveTarget(_moveTarget);
			monster.speed = _monsterData.speed;
			monster.hp = _monsterData.maxHP;
			monster.SetPool(_monsterPool);

			if (monster.GetComponent<Rigidbody>() == null)
			{
				monster.AddComponent<Rigidbody>().useGravity = false;
			}
		}
	}

	public void ReturnToPool(Monster monster)
	{
		_monsterPool.ReturnObject(monster);
	}

	private void OnDestroy()
	{
		if (_monsterPool != null)
		{
			_monsterPool.ClearPool();	
		}
	}
}
