public interface ITowerStrategy
{
    void Initialize(Towers tower, TowerDataSO towerData);
    void Update();
    void OnSpawn();
    void OnDestroy();
}
