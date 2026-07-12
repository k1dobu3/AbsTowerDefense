using UnityEngine;

[CreateAssetMenu(fileName = "AmmoSO", menuName = "Scriptable Objects/AmmoSO")]
public class AmmoSO : ScriptableObject
{
    [Header ("Ammo Data")]
    public string ammoName = "DefautBullet";
    public float ammoSpeed = 10f;
    public float ammoDamage = 25f;
    public GameObject ammoProjectilePrefab; 
}
