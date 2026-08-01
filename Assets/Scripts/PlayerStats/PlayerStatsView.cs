using UnityEngine;
using UnityEngine.UIElements;
using AbsTowerDefense.PlayerStats.Abstract;

namespace AbsTowerDefense.PlayerStats
{
	public class PlayerStatsView : MonoBehaviour, IPlayerStatsView
	{
		[SerializeField]
		private UIDocument uiDocument;
		private Label _killsCountLabel;
		private Label _spawnerTimerLabel;
		
		public void Initialize()
		{
			if (uiDocument == null)
			{
				uiDocument = GetComponent<UIDocument>();
			}

			var root = uiDocument.rootVisualElement;
			_spawnerTimerLabel = root.Q<Label>("MonsterSpawnTimer");
			_killsCountLabel = root.Q<Label>("KillCounter");
		}

		public void StatsUpdate(PlayerStatsModel model)
		{
			_killsCountLabel.text = $"Kills: {model.Kills}";
			Debug.Log($"Kills: {model.Kills}");
		}

		public void TimersUpdate(PlayerStatsModel model)
		{
			_spawnerTimerLabel.text = $"👾 spawn: {model.TimeToNextSpawnMonster:F2}";
		}
	}
}