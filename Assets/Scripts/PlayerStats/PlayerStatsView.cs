using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsView : MonoBehaviour, IPlayerStatsView
{
    [SerializeField] private UIDocument uiDocument;
    private Label _spawnerTimerLabel;

    public void Initialize()
    {
        if (uiDocument == null)
        {
            uiDocument = GetComponent<UIDocument>();
        }

        var root = uiDocument.rootVisualElement;
        _spawnerTimerLabel = root.Q<Label>("LABEL_MonsterSpawnTimer");
    }

    public void StatsUpdate(PlayerStatsModel model)
    {
        _spawnerTimerLabel.text = $"Next spawn: {model.TimeToNextSpawnMonster}";
    }

    public event Action OnSomeButtonClicked;
}
