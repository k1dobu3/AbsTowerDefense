using System;
using UnityEngine;

public class PlayerStatsModel
{
    public int Money { get; private set; } = 150;
    public int Score { get; private set; } = 0;
    public int Kills { get; private set; } = 0;
    public float TimeToNextSpawnMonster { get; private set; } = 30f;

    public event Action OnStatsChanged;

    public void UpdateTimer(float currentTimeToNexSpawn)
    {
        TimeToNextSpawnMonster = currentTimeToNexSpawn;
        OnStatsChanged?.Invoke();
    }
}
