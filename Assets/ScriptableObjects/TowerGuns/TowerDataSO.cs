using UnityEngine;

[CreateAssetMenu(fileName = "TowerDataSO", menuName = "Scriptable Objects/TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
	[Header("Tower Common Data")]
	public string towerName = "Tower";
	public float maxHP = 100f;
	public float fireRange = 10f;
	public float fireSpeedCD = 2f;
	public bool towerGunHeadMoveable = false;
	public float minBarrelAngle = 30f;
	public float maxBarrelAngle = 80f;

	[Header("Tower Upgrade Data")]
	public float rotationSpeed = 5f;
	public float barrelRotationSpeed = 0f;
	public float startMuzzleSpeed = 0f;
}
