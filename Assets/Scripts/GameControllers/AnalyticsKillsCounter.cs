using System;
using UnityEngine;
using AbsTowerDefense.MonsterLogic;

public class AnalyticsKillsCounter : MonoBehaviour
{
	public static AnalyticsKillsCounter Instance { get; private set; }

	public event Action<int> OnKillsCountChanged;
	public int _gameKills = 0;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		Monster.OnAnyMonsterDeath += AddKillsScore;
	}

	private void OnDisable()
	{
		Monster.OnAnyMonsterDeath -= AddKillsScore;
	}

	public void AddKillsScore()
	{
		_gameKills++;
		OnKillsCountChanged?.Invoke(_gameKills);
	}
}