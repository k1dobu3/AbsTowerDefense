using AbsTowerDefense.MonsterLogic.Abstract;
using UnityEngine;

namespace AbsTowerDefense.TowersLogic.Abstract
{
	public interface ITargetable
	{
		IDamageable FindTarget(Vector3 searcherPos, float fireRange);
	}
}
