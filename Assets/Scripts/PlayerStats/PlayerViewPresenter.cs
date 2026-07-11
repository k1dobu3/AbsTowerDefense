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
    }

    public void Start()
    {
        _view.Initialize();
        UpdateView();
    }

    private void UpdateView()
    {
        _view.StatsUpdate(_model);
    }

    public void Dispose()
    {
        _model.OnStatsChanged -= UpdateView;
    }
}
