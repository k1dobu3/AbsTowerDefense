using AbsTowerDefense.GameObjectPool;
using AbsTowerDefense.MonsterLogic.Abstract;

namespace AbsTowerDefense.TowersLogic
{
	public class ProjectileFactory<T> where T : BaseProjectile
	{
		private readonly GameObjectPool<T> _projectilePool;
		private readonly AmmoSO _projectileData;

		public ProjectileFactory(GameObjectPool<T> projectilePool, AmmoSO projectileData)
		{
			_projectilePool = projectilePool;
			_projectileData = projectileData;
		}

		public T CreateProjectile(IDamageable target = null)
		{
			T projectile = _projectilePool.GetObject();
			if (projectile == null)
			{
				return null;
			}
			projectile.Initialize(_projectileData, target);
			return projectile;
		}

		public void ReturnToPool(T projectile)
		{
			if (projectile != null)
			{
				_projectilePool.ReturnObject(projectile);
			}
		}
	}
}