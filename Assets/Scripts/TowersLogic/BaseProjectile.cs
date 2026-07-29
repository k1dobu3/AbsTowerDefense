using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour, IPoolable
{
	public float damage;
	public abstract void OnSpawn();
	public abstract void OnDespawn();
}
