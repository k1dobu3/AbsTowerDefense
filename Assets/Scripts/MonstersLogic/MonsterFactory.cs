using UnityEngine;
using AbsTowerDefense.GameObjectPool;

namespace AbsTowerDefense.MonsterLogic
{
	public class MonsterFactory
	{
		private readonly GameObjectPool<Monster> _monsterPool;
		private readonly MonsterDataSO _monsterData;

		public MonsterFactory(GameObjectPool<Monster> monsterPool, MonsterDataSO monsterData)
		{
			_monsterPool = monsterPool;
			_monsterData = monsterData;
		}

		public Monster CreateMonster(Vector3 spawnPosition, Collider monsterGoTo)
		{
			Monster monster = _monsterPool.GetObject();
			if (monster == null)
			{
				return null;
			}
			monster.Initialize(spawnPosition, monsterGoTo, _monsterData);
			return monster;
		}

		public void ReturnToPool(Monster monster)
		{
			if (monster != null)
			{
				_monsterPool.ReturnObject(monster);
			}
		}
	}
}