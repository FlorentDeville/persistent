using AssemblyCSharp;
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

                //Get the target
                int id = Random.Range(0, turnMng.m_PlayerPawns.Count);
                GameObject obj = turnMng.m_PlayerPawns[id];
                actions.m_Attack.SetTarget(obj);
                m_Behavior.SetSelectedAction(actions.m_Attack);
                m_Runner.SetCurrentState((int)RunSingleTurnState.RunAction, "AI has selected action");
            }
        }
    }
}
