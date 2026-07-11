using UnityEngine;
using System.Collections;

public class GuidedProjectile : MonoBehaviour {
	public GameObject m_target;
	public float m_speed = 0.2f;
	public float m_damage = 1500;

	void Update () {
		if (m_target == null || !m_target.gameObject.activeInHierarchy) {
			Destroy (gameObject);
			return;
		}

		var translation = m_target.transform.position - transform.position;
		if (translation.magnitude > m_speed) {
			translation = translation.normalized * m_speed;
		}
		transform.Translate (translation);
	}

	void OnTriggerEnter(Collider other) {

		GetComponent<Collider>().enabled = false;

		var monster = other.gameObject.GetComponent<Monster> ();
		if (monster == null)
			return;

		monster.TakeDamage (1000000f, false);
		Destroy (gameObject);
	}
}
