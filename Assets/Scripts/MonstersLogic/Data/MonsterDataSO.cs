using UnityEngine;

namespace AbsTowerDefense.MonsterLogic
{
	[CreateAssetMenu(fileName = "MonsterDataSO", menuName = "Scriptable Objects/MonsterDataSO")]
	public class MonsterDataSO : ScriptableObject
	{
		[Header("Monster Common Data")]
		public string monsterName = "Labubu";
		public float maxHP;
		[Range(2f, 8f)]
		public float speed = 5f;
		public int tokenPrice = 1;
		public GameObject monsterPrefab;
	}	
}