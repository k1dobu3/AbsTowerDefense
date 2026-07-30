using UnityEngine;

namespace AbsTowerDefense.MonsterLogic
{
	[CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Scriptable Objects/MonsterDataSO")]
	public class MonsterDataSO : ScriptableObject
	{
		[Header("Monster Common Data")]
		public string monsterName = "Labubu";
		public float maxHP;
		[Range(1f, 10f)]
		public float speed = 0.1f;
		public int tokenPrice = 1;
		public GameObject monsterPrefab;
	}	
}