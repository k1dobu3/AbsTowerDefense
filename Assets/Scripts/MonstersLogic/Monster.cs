using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour, IPoolable {

	private int _tokenPrice = 1;
	public GameObject _moveTarget;
	private float _speed = 0.1f;
	private float _hp = 100;
	const float _reachDistance = 0.5f;
	private GameObjectPool<Monster> _pool;

	public float speed { get { return _speed; } set { _speed = value; } }
	public float hp { get { return _hp; } set { _hp = value; } }

	public void SetPool(GameObjectPool<Monster> pool) 
	{
		_pool = pool;
	}
	
	public void OnEnable() {

	}
	public void OnDisable() {
		_moveTarget = null;
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
		_moveTarget = target;
	}

	void Update () {
		if (_moveTarget == null)
			return;
		
		if (Vector3.Distance (transform.position, _moveTarget.transform.position) <= _reachDistance) {
			OnDisable();
			return;
		}

		var translation = _moveTarget.transform.position - transform.position;
		if (translation.magnitude > _speed) {
			translation = translation.normalized * 	_speed;
		}
		transform.Translate (translation);
	}
}
