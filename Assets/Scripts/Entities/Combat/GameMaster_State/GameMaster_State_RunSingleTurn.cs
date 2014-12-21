using AssemblyCSharp;
using UnityEngine;
using Assets.Scripts.Component.Actions;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        private FSMRunner m_InnerRunner;

        private IAction m_SelectedAction;

        public enum RunSingleTurnState
        {
            Prepare,
            PlayerTurn,
            AITurn,
            RunAction,
            Resolve,
            Over
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
            m_InnerRunner.RegisterState<RunSingleTurn_State_Over>();
        }

        public override void OnEnter()
        {
            m_InnerRunner.StartState((int)RunSingleTurnState.Prepare);
        }

        public override void OnExecute()
        {
            m_InnerRunner.Update();
            if(m_InnerRunner.GetCurrentState() == (int)RunSingleTurnState.Over)
            {
                m_Runner.SetCurrentState((int)GameMasterState.CheckGoal, "turn over");
            }
        }

        public void SetSelectedAction(IAction _act)
        {
            RunSingleTurn_State_PlayerTurn playerTurn = GetStateObject<RunSingleTurn_State_PlayerTurn>(RunSingleTurnState.PlayerTurn);
            playerTurn.SetSelectedAction();
            m_SelectedAction = _act;
        }

        public IAction GetSelectedAction()
        {
            return m_SelectedAction;
        }

        public StateType GetStateObject<StateType>(RunSingleTurnState _state)
            where StateType : IState
        {
            return m_InnerRunner.GetStateObject<StateType>((int)_state);
        }
    }
}
