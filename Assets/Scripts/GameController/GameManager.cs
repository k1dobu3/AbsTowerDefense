using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSpawner spawner;
    public static GameManager Instance { get; private set; }

    public static event Action<int> OnKillsCountChanged;
    private float _playTime;
    public static int _gameKills = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log(_gameKills);
    }

    private void OnEnable()
    {
        Monster.OnAnyMonsterDeath += AddKillsScoreOne;
    }

    private void OnDisable()
    {
        Monster.OnAnyMonsterDeath -= AddKillsScoreOne;
    }

    public void AddKillsScoreOne()
    {
        AddKillsScore(1);
    }

    public static void AddKillsScore(int kills)
    {
        _gameKills += kills;
        OnKillsCountChanged?.Invoke(_gameKills);
    }

    private void Update()
    {
        _playTime += Time.deltaTime;
    }
}

internal class MonsterSpawner
{
}