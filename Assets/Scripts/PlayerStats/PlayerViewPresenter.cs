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
        _model.OnTimerChanged += UpdateViewTimer;
    }

    public void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKillsCountChanged += UpdateModelKills;
            UpdateModelKills(GameManager.Instance._gameKills);
        }
        
        _view.Initialize();
        UpdateView();
    }

    public void UpdateViewTimer()
    {
        _view.TimersUpdate(_model);
    }

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKillsCountChanged -= UpdateModelKills;
        }
    }
}
