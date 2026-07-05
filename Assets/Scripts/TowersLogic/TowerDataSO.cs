using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataSO", menuName = "Scriptable Objects/TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
    [Header("Tower Common Data")]
    public string towerName = "Tower";
    public float maxHP = 100f;
    public float damage = 1f;
    public float fireRange = 10f;
    public float fireSpeed = 2f;
    public GameObject projectilePrefab;
    public bool towerGunHeadMoveable = false;

    [Header("Tower Upgrade Data")]
    public float rotationSpeed = 5f;
}
