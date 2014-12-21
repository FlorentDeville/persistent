using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        private FSMRunner m_InnerRunner;

        public enum RunSingleTurnState
        {
            Prepare,
            PlayerTurn,
            AITurn,
            RunAction,
            Resolve
        }

        public override int State { get { return (int)GameMasterState.RunSingleTurn; } }

        public override void Initialize()
        {
            m_InnerRunner = new FSMRunner(m_GameObject);
            m_InnerRunner.RegisterState<RunSingleTurn_State_Prepare>();
            m_InnerRunner.RegisterState<RunSingleTurn_State_PlayerTurn>();
            m_InnerRunner.RegisterState<RunSingleTurn_State_AITurn>();
            m_InnerRunner.RegisterState<RunSingleTurn_State_RunAction>();
            m_InnerRunner.RegisterState<RunSingleTurn_State_Resolve>();
        }

        public override void OnEnter()
        {
            m_InnerRunner.StartState((int)RunSingleTurnState.Prepare);
        }

        public override void OnExecute()
        {
            m_InnerRunner.Update();
        }

        public StateType GetStateObject<StateType>(RunSingleTurnState _state)
            where StateType : IState
        {
            return m_InnerRunner.GetStateObject<StateType>((int)_state);
        }
    }
}
