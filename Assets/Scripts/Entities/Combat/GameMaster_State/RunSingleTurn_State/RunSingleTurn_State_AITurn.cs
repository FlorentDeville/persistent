using AssemblyCSharp;
using Assets.Scripts.Entities.Combat;
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
                PawnActions actions = m_Behavior.m_TurnManager.GetCurrentPawn().GetComponent<PawnActions>();

                //Get the target
                int id = Random.Range(0, m_Behavior.m_TurnManager.m_PlayerPawns.Count);
                GameObject obj = m_Behavior.m_TurnManager.m_PlayerPawns[id];
                actions.m_Attack.SetTarget(obj);
                m_Behavior.SetSelectedAction(actions.m_Attack);
                m_Runner.SetCurrentState((int)RunSingleTurnState.RunAction, "AI has selected action");
            }
        }
    }
}
