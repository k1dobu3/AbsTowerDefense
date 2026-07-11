using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] public PlayerStatsView uiObject;
    private PlayerStatsModel _model;
    private PlayerViewPresenter _presenter;

    public PlayerStatsModel GetPlayerStatsModel() => _model;

    private void Start()
    {
        _model = new PlayerStatsModel();
        _presenter = new PlayerViewPresenter(_model, uiObject);
        _presenter.Start();
    }

    // private void Update()
    // {
    // }

    private void OnDestroy()
    {
        _presenter?.Dispose();
    }
}
