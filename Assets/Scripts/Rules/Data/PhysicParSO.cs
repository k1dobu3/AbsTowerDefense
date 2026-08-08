using UnityEngine;

namespace AbsTowerDefense.Rules
{
	[CreateAssetMenu(fileName = "PhysicParSO", menuName = "Scriptable Objects/PhysicParSO")]
	public class PhysicParSO : ScriptableObject
	{
		[SerializeField]
		public string worldName = "Earth";
		public float gravityG = 9.81f;
	}
}