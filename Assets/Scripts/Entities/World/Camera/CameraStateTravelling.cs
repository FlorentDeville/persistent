using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CameraStateTravelling : IFSMState<CameraBehavior>
	{
		private bool m_TravelOver;
		
		private float m_StartTime;
		
		private Vector3 m_EndTarget;
		
		private Vector3 m_StartTarget;
		
		public bool TravelOver 
		{
			get {return m_TravelOver;}
			set { m_TravelOver = value;}
		}

        public override int State { get { return (int)CameraState.eTravelling; } }
		
		public override void OnEnter ()
		{
			m_TravelOver = false;
			m_StartTime = Time.time;
			
			m_StartTarget = m_GameObject.transform.position;
			m_EndTarget = (Vector3)m_Runner.StateParameter - new Vector3(0, 0, 1) * m_Behavior.m_CameraDistance + new Vector3(0, 1, 0) * m_Behavior.m_CameraHeight;
			base.OnEnter ();
		}
		
		public override void OnExecute()
		{
			float Parameter = (Time.time - m_StartTime) / m_Behavior.m_TravellingDuration;
			float SmoothParameter =  Mathf.SmoothStep(0, 1, Parameter);
			
			if(SmoothParameter >= 1)
			{
				m_TravelOver = true;
				return;
			}
			
			Vector3 NewPosition = Vector3.Lerp(m_StartTarget, m_EndTarget, SmoothParameter);
			m_GameObject.transform.position = NewPosition;
			
			base.OnExecute();
		}
	}
}

