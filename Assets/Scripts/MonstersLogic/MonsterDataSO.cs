using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Scriptable Objects/MonsterDataSO")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Monster Common Data")]
    public string monsterName = "Labubu";
    public float maxHP = 100f;
    public float speed = 0.1f;
    public int tokenPrice = 1;
    public Monster monsterPrefab;
}
