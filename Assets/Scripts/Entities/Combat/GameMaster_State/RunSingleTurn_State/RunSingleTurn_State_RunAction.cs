using AssemblyCSharp;
using UnityEngine;
using Assets.Scripts.Component.Actions;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_RunAction : IFSMState<GameMaster>
        {
            private ActionRunner m_SelectedAction;

            public override int State { get { return (int)RunSingleTurnState.RunAction; } }

            public override void OnEnter()
            {
                m_SelectedAction = m_Behavior.GetRunSingleTurnState().GetSelectedAction();
                m_SelectedAction.Prepare();
                m_Behavior.m_UIEffects.StartAnimationShowAttackName(m_SelectedAction.ActionDescription.m_DisplayName);
            }

            public override void OnLateExecute()
            {
                if (m_SelectedAction.Execute() == Result.Over)
                    m_Runner.SetCurrentState((int)RunSingleTurnState.Resolve, "action is over");
            }
        }
    }
}
