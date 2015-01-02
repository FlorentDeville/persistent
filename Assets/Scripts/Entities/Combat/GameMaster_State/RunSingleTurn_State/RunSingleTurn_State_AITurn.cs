using AssemblyCSharp;
using Assets.Scripts.Component.Actions;
using Assets.Scripts.Entities.Combat;
using Assets.Scripts.Manager;
using UnityEngine;


public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_AITurn : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.AITurn; } }

            public override void OnEnter()
            {
            }

            public override void OnExecute()
            {
                GameTurnManager turnMng = GameTurnManager.GetInstance();
                PawnActions actions = turnMng.GetCurrentPawn().GetComponent<PawnActions>();
                ActionRunner selectedAction = actions.m_DefaultAttack;

                //Set target
                int id = Random.Range(0, turnMng.m_PlayerPawns.Count);
                GameObject obj = turnMng.m_PlayerPawns[id];
                selectedAction.m_Target = obj;
                selectedAction.ActionDescription = turnMng.GetCurrentPawn().GetComponent<PawnBehavior>().m_AttackDescription;

                //Set the action to run and go to next state
                m_Behavior.SetSelectedAction(selectedAction);
                m_Behavior.ActionReady();
                m_Runner.SetCurrentState((int)RunSingleTurnState.RunAction, "AI has selected action");
            }
        }
    }
}
