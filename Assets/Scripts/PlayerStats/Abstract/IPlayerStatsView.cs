using UnityEngine;

public interface IPlayerStatsView
{
    void Initialize();
    void StatsUpdate(PlayerStatsModel model);
}
