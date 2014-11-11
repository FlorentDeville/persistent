using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class IFSMState 
	{
		protected int m_State;
		
		protected FSMRunner m_Runner;
		
		protected GameObject m_GameObject;
		
		public virtual void OnEnter(){}
		
		public virtual void OnExecute(){}
		
		public virtual void OnExit(){}
	
	}
}
