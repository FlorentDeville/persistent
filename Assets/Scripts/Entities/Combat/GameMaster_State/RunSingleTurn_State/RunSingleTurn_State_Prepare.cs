using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_Prepare : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.Prepare; } }

            public override void OnEnter()
            {
                GameObject obj = m_Behavior.m_TurnManager.GetCurrentPawn();

                PawnStatistics stat = obj.GetComponent<PawnStatistics>();

                if (stat.m_IsControlledByPlayer)
                    m_Runner.SetCurrentState((int)RunSingleTurnState.PlayerTurn, "Player turn");
                else
                    m_Runner.SetCurrentState((int)RunSingleTurnState.AITurn, "Enemy turn");
            }
        }
    }
}
