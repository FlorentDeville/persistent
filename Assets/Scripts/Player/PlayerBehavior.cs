using UnityEngine;
using System.Collections;

using AssemblyCSharp;

namespace Persistent
{
	public class PlayerBehavior : MonoBehaviour 
	{
		public float m_SlowSpeed;
		public float m_NormalSpeed;

        public bool m_InputEnabled;
        public bool m_OutlinedMaterialEnabled;

        public Material m_DefaultMaterial;
        public Material m_OutlinedMaterial;
		
		private FSMRunner m_Runner;
		
		// Use this for initialization
		void Start () 
		{
			m_Runner = new FSMRunner();
			
			PlayerStateWorld StateWorld = new PlayerStateWorld(m_Runner, this.gameObject);
			
			m_Runner.AddState((int)PlayerState.eWorld, StateWorld);
			
			m_Runner.SetCurrentState((int)PlayerState.eWorld, "set initial state");
		}
		
		// Update is called once per frame
		void Update () 
		{
			m_Runner.Update();
		}
	}
}