using System;
using UnityEngine;

public class PlayerStatsModel
{
    public int Money { get; private set; } = 150;
    public int Score { get; private set; } = 0;
    public int Kills { get; private set; } = 0;
    public float TimeToNextSpawnMonster { get; private set; } = 30f;

    public event Action OnStatsChanged;
    public event Action OnTimerChanged;

    public void UpdateKills(int currentKills)
    {
        Kills = currentKills;
        OnStatsChanged?.Invoke();
    }

    public void UpdateTimer(float currentTimeToNexSpawn)
    {
        TimeToNextSpawnMonster = currentTimeToNexSpawn;
        OnStatsChanged?.Invoke();
    }
}
