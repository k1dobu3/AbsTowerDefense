using UnityEngine;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;
using Unity.VisualScripting;

namespace AbsTowerDefense.TowersLogic
{
	public class SearchTargetBehaviour : MonoBehaviour, ITargetable
	{
		public IDamageable FindTarget(Vector3 searcherPos, float fireRange)
		{
			Collider[] colliders = Physics.OverlapSphere(searcherPos, fireRange);
			IDamageable nearestTarget = null;

			foreach (var col in colliders)
			{
				if (col.CompareTag("Monster"))
				{
					float distance = (searcherPos - col.transform.position).sqrMagnitude;
					if (distance < fireRange*fireRange)
					{
						nearestTarget = col.GetComponent<IDamageable>();
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