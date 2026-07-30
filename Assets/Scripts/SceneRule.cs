using UnityEngine;

namespace AbsTowerDefense.Rules
{
	public class SceneRule : MonoBehaviour
	{
		[SerializeField] 
		public PhysicParSO _currentPhysicRule;
		private float _sceneGravity;
		public static SceneRule Instance { get; private set; }
		public float sceneGravity { get { return _sceneGravity; } set { _sceneGravity = value; } }

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
			sceneGravity = _currentPhysicRule.gravityG;
			Physics.gravity = new Vector3(0, -sceneGravity, 0);
		}
	}
}