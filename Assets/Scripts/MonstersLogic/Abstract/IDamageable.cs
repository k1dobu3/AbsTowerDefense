using UnityEngine;

namespace AbsTowerDefense.MonsterLogic.Abstract
{
	public interface IDamageable
	{	
		bool IsAlive { get; }
		Transform Transform { get; }
		float Speed { get; }
	}
}