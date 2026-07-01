using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour, IPoolable {

	public GameObject m_moveTarget;
	public float m_speed = 0.1f;
	public int m_maxHP = 30;
	const float m_reachDistance = 0.3f;
	public int m_hp;
	private GameObjectPool<Monster> _pool;

	public void SetPool(GameObjectPool<Monster> pool) 
	{
		_pool = pool;
	}
	
	public void OnEnable() {
		m_hp = m_maxHP;
	}
	public void OnDisable() {
		m_moveTarget = null;
		if (_pool != null) 
		{
			_pool.ReturnObject(this);
			Debug.Log($"[Monster] {gameObject.GetEntityId()} is disabled and returned to pool");
		}
		else 
		{
			Debug.LogWarning($"[Monster] {gameObject.GetEntityId()} is disabled but pool is null");
		}
	}

	public void SetMoveTarget(GameObject target) 
	{
		m_moveTarget = target;
	}

	void Update () {
		if (m_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, m_moveTarget.transform.position) <= m_reachDistance) {
			OnDisable();
			return;
		}

		var translation = m_moveTarget.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}
}
