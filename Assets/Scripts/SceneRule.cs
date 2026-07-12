using System;
using UnityEngine;

public class SceneRule : MonoBehaviour
{
    [SerializeField] public PhysicParSO _currentPhysicRule;
    private float _sceneGravity;
    public static SceneRule Instance { get; private set; }
    public float SceneGravity { get { return _sceneGravity; } set { _sceneGravity = value;} }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneGravity = _currentPhysicRule.gravityG;
    }

}
