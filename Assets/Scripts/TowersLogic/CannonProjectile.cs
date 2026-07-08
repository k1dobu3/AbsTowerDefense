using UnityEngine;
using System.Collections;

public class CannonProjectile : MonoBehaviour {
	public float _speed = 0.2f;
	public int _damage = 10;

	void Update () {
		var translation = transform.forward * _speed;
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {
		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.TakeDamage (_damage);
		Destroy (gameObject);
	}
}
