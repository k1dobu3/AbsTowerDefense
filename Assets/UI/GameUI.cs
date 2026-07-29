using UnityEngine;

public class GameUI : MonoBehaviour
{
	[SerializeField] public PlayerStatsView uiObject;
	private PlayerStatsModel _model;
	private PlayerViewPresenter _presenter;

	public PlayerStatsModel GetPlayerStatsModel() => _model;

	private void Awake()
	{
		_model = new PlayerStatsModel();
		_presenter = new PlayerViewPresenter(_model, uiObject);
	} 

	private void Start()
	{
		_presenter.Initialize();
	}

	private void Update()
	{
		_presenter?.UpdateViewMonsterTimer();
	}

	private void OnDestroy()
	{
		_presenter?.Dispose();
	}
}
