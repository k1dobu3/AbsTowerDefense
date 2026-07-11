using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<int> OnKillsCountChanged;
    private float _playTime;
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

    private void Update()
    {
        _playTime += Time.deltaTime;
    }
}