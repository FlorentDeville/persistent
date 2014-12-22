using AssemblyCSharp;
using Assets.Scripts.Manager;

using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_CheckGoal : IFSMState<GameMaster>
    {
        private GameTurnManager m_TurnManager;

        public override int State { get { return (int)GameMasterState.CheckGoal; } }

        public override void Initialize()
        {
            m_TurnManager = GameTurnManager.GetInstance();
        }

        public override void OnEnter()
        {
            if (AreAllPlayerPawnsDead())
                m_Runner.SetCurrentState((int)GameMasterState.GameOver, "all players dead");
            else if (AreAllEnemiesDead())
                m_Runner.SetCurrentState((int)GameMasterState.Victory, "all enemies dead");
            else
            {
                m_Runner.SetCurrentState((int)GameMasterState.RunSingleTurn, "");
                m_TurnManager.NextTurn();
            }
        }

        private bool AreAllEnemiesDead()
        {
            foreach (GameObject obj in m_TurnManager.m_EnemiesPawns)
            {
                PawnStatistics stat = obj.GetComponent<PawnStatistics>();
                if (stat.m_HP > 0)
                    return false;
            }

            return true;
        }

        private bool AreAllPlayerPawnsDead()
        {
            foreach (GameObject obj in m_TurnManager.m_PlayerPawns)
            {
                PawnStatistics stat = obj.GetComponent<PawnStatistics>();
                if (stat.m_HP > 0)
                    return false;
            }

            return true;
        }
    }
}
