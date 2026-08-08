using UnityEngine;
using AbsTowerDefense.TowersLogic.Abstract;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class SearchTargetBehaviour : MonoBehaviour, ITargetable
	{
		private IDamageable _nearestTarget;

		public IDamageable FindTarget(Vector3 searcherPos, float fireRange)
		{
			Collider[] colliders = Physics.OverlapSphere(searcherPos, fireRange);

			foreach (var col in colliders)
			{
				if (_nearestTarget != null)
				{
					if (_nearestTarget.Transform == null || !_nearestTarget.IsAlive)
					{
						_nearestTarget = null;
					}
					else
					{
						float dist = Vector3.Distance(searcherPos, _nearestTarget.Transform.position);
						if (dist > fireRange)
						{
							_nearestTarget = null;
						}
						else
						{
							return _nearestTarget;
						}
					}
				}
				if (col.CompareTag("Monster"))
				{
					_nearestTarget = col.GetComponent<IDamageable>();
					break;
				}
				else
				{
					_nearestTarget = null;
				}
			}
			return _nearestTarget;
		}
	}
}