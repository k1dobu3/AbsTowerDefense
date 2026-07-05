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
        else
        {
            AimTarget();
            //TryShootAtTarget();
        }
    }

    private void FindTarget()
    {
        Vector3 timeTarget = new Vector3(-5.17f, 0.5f, 14.04f);
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, timeTarget);

        if (distance <= _currentTower.fireRange)
        {
            _target = GameObject.FindWithTag("Monster")?.transform;
            Debug.Log($"[Towers] Distance to target: {_target?.transform.position}, Target found: {_target?.name}");
        }
        else
        {
            _target = null;
            Debug.Log($"[Towers] Distance to target: {distance}, possible range: {_currentTower.fireRange}, Target not found");
        }
    }


    private float _minAimAngle = -20f;
    private float _maxAimAngle = 45f;

    private void AimTarget()
    {
        Vector3 direction = _target.position - transform.position;

        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            float targetY = targetRotation.eulerAngles.y;
            float currentY = transform.rotation.eulerAngles.y;
            float newY = Mathf.LerpAngle(currentY, targetY, Time.deltaTime * _currentTower.rotationSpeed);

            transform.rotation = Quaternion.Euler(0, newY, 0);
        }
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
