using System;
using System.Collections.Generic;

using UnityEngine;

namespace AssemblyCSharp
{
	public class FSMRunner
	{
		#region Variables
		
		private Dictionary<int, IFSMState> m_StatesList;
		
		/// <summary>
		/// The state the machine is executing
		/// </summary>
		private int m_CurrentState;
		
		/// <summary>
		/// The previous state executed by the machine
		/// </summary>
		private int m_PreviousState;
		
		/// <summary>
		/// The next state the machine will execute
		/// </summary>
		private int m_DefferedCurrentState;
		
		/// <summary>
		/// The parameter for the deffered state
		/// </summary>
		private object m_DefferedStateParameter;
		
		/// <summary>
		/// The reason the state changed
		/// </summary>
		private string m_SetStateReason;
		
		/// <summary>
		/// Flag to know if the machine entered the state yet
		/// </summary>
		private bool m_EnteredState;
		
		/// <summary>
		/// The parameter for the current state
		/// </summary>
		private object m_StateParameter;
		
		/// <summary>
		/// Value indicating if the next state can be changed
		/// </summary>
		private bool m_NextStateLocked;
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AssemblyCSharp.FSMRunner"/> show logs.
		/// </summary>
		/// <value>
		/// <c>true</c> if show log; otherwise, <c>false</c>.
		/// </value>
		public bool ShowLog{get;set;}
		
		/// <summary>
		/// Gets the state parameter.
		/// </summary>
		/// <value>
		/// The state parameter.
		/// </value>
		public object StateParameter
		{
			get{return m_StateParameter;}
		}
		
		#endregion
		
		public FSMRunner ()
		{
			m_StatesList = new Dictionary<int, IFSMState>(); 
			m_CurrentState = 0;
			m_PreviousState = 0;
			m_DefferedCurrentState = 0; 
			m_EnteredState = false;
			ShowLog = false;
			
			m_DefferedStateParameter = null;
			m_StateParameter = null;
			
			m_NextStateLocked = false;
		}
		
		public void AddState(int _State, IFSMState _NewState)
		{
			m_StatesList.Add(_State, _NewState);
		}
		
		public void SetCurrentState(int _NewState, string _Reason)
		{
			if(m_NextStateLocked)
				return;
			m_DefferedCurrentState = _NewState;
			m_SetStateReason = _Reason;
			m_DefferedStateParameter = null;
		}
		
		public void SetCurrentState(int _NewState, string _Reason, object _Parameter)
		{
			if(m_NextStateLocked)
				return;
			m_DefferedCurrentState = _NewState;
			m_SetStateReason = _Reason;
			m_DefferedStateParameter = _Parameter;
		}
		
		/// <summary>
		/// Change thre state and lock it until it is being executed so it can't be changed
		/// </summary>
		/// <param name='_NewState'>
		/// The new state to execute
		/// </param>
		/// <param name='_Reason'>
		/// The reason we changed the state
		/// </param>
		/// <param name='_Parameter'>
		/// A parameter
		/// </param>
		public void ForceCurrentState(int _NewState, string _Reason, object _Parameter)
		{
			SetCurrentState(_NewState, _Reason, _Parameter);
			m_NextStateLocked = true;
		}
		
		public void SetImmediateCurrentState(int _NewState)
		{
			m_DefferedCurrentState = _NewState;
			m_CurrentState = _NewState;
		}
		
		public int GetCurrentState()
		{
			return m_CurrentState;
		}
		
		public int GetDefferedState()
		{
			return m_DefferedCurrentState;
		}
		
		public int GetPreviousState()
		{
			return m_PreviousState;
		}
		
		public void Update()
		{
			//if never entered the state
			if(!m_EnteredState)
			{
				if(ShowLog)
					Debug.Log("state changed because : " + m_SetStateReason);
				
				m_StatesList[m_CurrentState].OnEnter();
				m_StatesList[m_CurrentState].OnExecute();
				m_EnteredState = true;
				return;
			}
			else
				m_StatesList[m_CurrentState].OnExecute();
			
			//update deffered state
			if(m_DefferedCurrentState != m_CurrentState)
			{
				m_StatesList[m_CurrentState].OnExit();
				m_PreviousState = m_CurrentState;
				m_CurrentState = m_DefferedCurrentState;
				m_StateParameter = m_DefferedStateParameter;
				m_EnteredState = false;
				m_NextStateLocked = false;
				return;
			}
		}
	}
}

