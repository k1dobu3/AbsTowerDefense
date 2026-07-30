namespace AbsTowerDefense.PlayerStats.Abstract
{
	public interface IPlayerStatsView
	{
		void Initialize();
		void StatsUpdate(PlayerStatsModel model);
		void TimersUpdate(PlayerStatsModel model);
	}
}