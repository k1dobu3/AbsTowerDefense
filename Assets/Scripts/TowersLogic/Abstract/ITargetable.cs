using UnityEngine;

namespace AbsTowerDefense.TowersLogic.Abstract
{
	public interface ITargetable
	{
		GameObject FindTarget(Vector3 searcherPos, float fireRange);
	}
}
