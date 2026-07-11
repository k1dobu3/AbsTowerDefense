using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsView : MonoBehaviour, IPlayerStatsView
{
    [SerializeField] private UIDocument uiDocument;
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
        _killsCountLabel = root.Query<Label>("MonsterKillCounter").First();
    }

    public void StatsUpdate(PlayerStatsModel model)
    {
        //_killsCountLabel.text = $"👾 Kills: {model.Kills}"; // ПРОБЛЕМА 
        Debug.Log(model.Kills);
    }

    public void TimersUpdate(PlayerStatsModel model)
    {
        _spawnerTimerLabel.text = $"👾 spawn: {model.TimeToNextSpawnMonster:F2}";
        // Debug.Log(model.TimeToNextSpawnMonster);
    }

    public event Action OnSomeButtonClicked;
}
