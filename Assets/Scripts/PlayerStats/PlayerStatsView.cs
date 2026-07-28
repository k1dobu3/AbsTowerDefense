using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsView : MonoBehaviour, IPlayerStatsView
{
	[SerializeField] private UIDocument uiDocument;
	private Label _killsCountLabel;
	private Label _spawnerTimerLabel;
	private Label _allGameTimerLabel;

	public void Initialize()
	{
		if (uiDocument == null)
		{
			uiDocument = GetComponent<UIDocument>();
		}

		var root = uiDocument.rootVisualElement;
		_spawnerTimerLabel = root.Q<Label>("MonsterSpawnTimer");
		_allGameTimerLabel = root.Q<Label>("AllGameTimer");
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
		_allGameTimerLabel.text = $"U play {model.AllGameTime:F1} sec"; 
	}
}
