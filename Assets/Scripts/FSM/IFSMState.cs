using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public abstract class IState 
	{
        public abstract int State { get; }

        protected FSMRunner m_Runner;

        protected GameObject m_GameObject;

        public virtual void Internal_Initialize(FSMRunner _Runner, GameObject _GameObject)
        {
            m_Runner = _Runner;
            m_GameObject = _GameObject;
        }

        public abstract void Initialize();

        public abstract void OnEnter();

        public abstract void OnExecute();

        public abstract void OnExit();
	
	}

    public abstract class IFSMState<BehaviorClass> : IState
        where BehaviorClass : Component 
    {

        protected BehaviorClass m_Behavior;

        public sealed override void Internal_Initialize(FSMRunner _Runner, GameObject _GameObject)
        {
            base.Internal_Initialize(_Runner, _GameObject);
            m_Behavior = m_GameObject.GetComponent<BehaviorClass>();
        }

        public override void Initialize() { }

        public override void OnEnter() { }

        public override void OnExecute() { }

        public override void OnExit() { }
    }
}
