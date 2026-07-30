using UnityEngine;
using AbsTowerDefense.TowersLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class SearchTargetBehaviour : MonoBehaviour, ITargetable
	{
		public GameObject FindTarget(Vector3 searcherPos, float fireRange)
		{
			Collider[] colliders = Physics.OverlapSphere(searcherPos, fireRange);
			GameObject nearestTarget = null;

			foreach (var col in colliders)
			{
				if (col.CompareTag("Monster"))
				{
					float distance = (searcherPos - col.transform.position).sqrMagnitude;
					if (distance < fireRange*fireRange)
					{
						nearestTarget = col.gameObject;
					}
					else
					{
						nearestTarget = null;
					}
				}
			}

			return nearestTarget;
		}
	}
}