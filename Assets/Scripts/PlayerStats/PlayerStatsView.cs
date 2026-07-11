using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsView : MonoBehaviour, IPlayerStatsView
{
    [SerializeField] private UIDocument uiDocument;
    private Label _killsCount;
    private Label _spawnerTimerLabel;

    public void Initialize()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        var root = uiDocument.rootVisualElement;
        _spawnerTimerLabel = root.Q<Label>("LABEL_MonsterSpawnTimer");
        _killsCount = root.Q<Label>("LABEL_MonsterKillCounter");
    }

    public void StatsUpdate(PlayerStatsModel model)
    {
        //_killsCount.text = $"Kills: {model.Kills}";
        Debug.Log(model.Kills);
    }

    public void TimersUpdate(PlayerStatsModel model)
    {
        _spawnerTimerLabel.text = $"Next spawn: {model.TimeToNextSpawnMonster:F2}";
        // Debug.Log(model.TimeToNextSpawnMonster);
    }

    public event Action OnSomeButtonClicked;
}
