using UnityEngine;

namespace AbsTowerDefense.MonsterLogic.Abstract
{
	public interface IDamageable
	{	
		bool IsAlive { get; }
		bool IsDead();
		Transform Transform { get; }
		float Speed { get; }
	}
}