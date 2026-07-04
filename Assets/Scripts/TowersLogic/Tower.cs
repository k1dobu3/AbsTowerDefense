using Unity.VisualScripting;
using UnityEngine;

public class Towers : MonoBehaviour, IPoolable
{
    [SerializeField] 
    private TowerDataSO _data;

    private TowerDataSO _currentTower;
    private ITowerStrategy _currentTowerStrategy;
    private float _currentHP;

    private void Awake()
    {
        if (_data == null)
        {
            Debug.Log($"Tower component found on {gameObject.name}");
            return;
        }

        Initaialize(_data);
    }

    public void Initaialize(TowerDataSO data)
    {
        _currentTower = data;
    
    }

    public void Update ()
    {
        _currentTowerStrategy?.Update();
    }

    public void OnEnable() {
		_currentTowerStrategy?.OnSpawn();
	}

	public void OnDisable() {
        _currentTowerStrategy?.OnDestroy();
	}

    public Transform Transform => transform;
}
