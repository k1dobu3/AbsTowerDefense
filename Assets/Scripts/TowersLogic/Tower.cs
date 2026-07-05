using Unity.VisualScripting;
using UnityEngine;

public class Towers : MonoBehaviour, IPoolable
{
    [SerializeField] 
    private TowerDataSO _data;

    private TowerDataSO _currentTower;
    private float _currentHP;
    private Transform _target;

    private void Awake()
    {
        if (_data == null)
        {
            Debug.Log($"Tower component found on {gameObject.name}");
            return;
        }

        Initaialize(_data);
    }

    public void Initaialize(TowerDataSO data)
    {
        _currentTower = data;
    }

    public void Update ()
    {
        if (_currentTower.projectilePrefab == null)
        {
             Debug.LogWarning($"[Towers] Projectile prefab is not found for {gameObject.name}");
             return;
        }

        if (_target == null)
        {
            FindTarget();
            return;
        }
        // else
        // {
        //     AimTarget();
        //     TryShootAtTarget();
        // }
    }

    private void FindTarget()
    {
        Vector3 timeTarget = new Vector3(-5.17f, 0.5f, 14.04f);
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, timeTarget);
        //Debug.Log($"Distance to target: {distance}");
    }

    // void Update () 
    // {
	// 	if (m_projectilePrefab == null)
	// 		return;

	// 	foreach (var monster in FindObjectsByType<Monster>()) {
	// 		if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
	// 			continue;

	// 		if (m_lastShotTime + m_shootInterval > Time.time)
	// 			continue;

	// 		// shot
	// 		var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;
	// 		var projectileBeh = projectile.GetComponent<GuidedProjectile> ();
	// 		projectileBeh.m_target = monster.gameObject;

	// 		m_lastShotTime = Time.time;
	// 	}
	
	// }

    public void OnEnable() {
	}

	public void OnDisable() {
	}

    public Transform Transform => transform;
}
