using UnityEngine;

public class PlayerViewPresenter
{
    private readonly PlayerStatsModel _model;
    private readonly PlayerStatsView _view;

    public PlayerViewPresenter(PlayerStatsModel model, PlayerStatsView view)
    {
        _model = model;
        _view = view;
        _model.OnStatsChanged += UpdateView;
        _model.OnTimerChanged += UpdateView;
    }

    public void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.OnKillsCountChanged += UpdateModelKills;
            UpdateModelKills(GameManager._gameKills);
        }
        
        _view.Initialize();
        UpdateView();
    }

    private void UpdateModelKills(int kills)
    {
        _model.UpdateKills(kills);
    }

    private void UpdateView()
    {
        _view.StatsUpdate(_model);
        UpdateViewTimer();
    }

    private void UpdateViewTimer()
    {
        _view.TimersUpdate(_model);
    }

    public void Dispose()
    {
        _model.OnStatsChanged -= UpdateView;
        if (GameManager.Instance != null)
        {
            GameManager.OnKillsCountChanged -= UpdateModelKills;
        }
    }
}
