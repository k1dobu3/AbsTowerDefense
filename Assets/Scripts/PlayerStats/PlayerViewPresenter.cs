using System;

namespace AbsTowerDefense.PlayerStats
{
	public class PlayerViewPresenter : IDisposable
	{
		private readonly PlayerStatsModel _model;
		private readonly PlayerStatsView _view;

		public PlayerViewPresenter(PlayerStatsModel model, PlayerStatsView view)
		{
			_model = model;
			_view = view;
			_model.OnStatsChanged += UpdateView;
			_model.OnMonsterSpawnTimerChanged += UpdateViewMonsterTimer;
		}

		public void Initialize()
		{
			if (AnalyticsKillsCounter.Instance != null)
			{
				AnalyticsKillsCounter.Instance.OnKillsCountChanged += UpdateModelKills;
				// GameManager.Instance.OnGameTimeWasChanged += UpdateModelGameTimer;
			}
			
			_view.Initialize();
			UpdateView();
		}

		public void UpdateViewMonsterTimer()
		{
			_view.TimersUpdate(_model);
		}

		// private void UpdateModelGameTimer(float gameTime)
		// {
		//     _model.UpdateGameTimer(gameTime);
		// }

		private void UpdateModelKills(int kills)
		{
			_model.UpdateKills(kills);
		}

		private void UpdateView()
		{
			_view.StatsUpdate(_model);
		}

		public void Dispose()
		{
			_model.OnStatsChanged -= UpdateView;
			_model.OnMonsterSpawnTimerChanged -= UpdateViewMonsterTimer;
			if (AnalyticsKillsCounter.Instance != null)
			{
				AnalyticsKillsCounter.Instance.OnKillsCountChanged -= UpdateModelKills;
				// GameManager.Instance.OnGameTimeWasChanged -= UpdateModelGameTimer;
			}
		}
	}
}