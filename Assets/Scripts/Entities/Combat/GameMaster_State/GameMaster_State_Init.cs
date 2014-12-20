using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Init : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.Init; } }

        public override void OnEnter()
        {
            //compute turns history
            m_Behavior.m_TurnManager.Init();
            m_Behavior.m_UITurnHistory.UpdateTurnHistory(m_Behavior.m_TurnManager);

            //init ui state
            int playerPawnCount = m_Behavior.m_TurnManager.m_PlayerPawns.Count;
            for (int i = 0; i < playerPawnCount; ++i)
            {
                GameObject pawn = m_Behavior.m_TurnManager.m_PlayerPawns[i];
                PawnUI pawnUIComponent = pawn.GetComponent<PawnUI>();
                if (pawnUIComponent == null)
                {
                    Debug.LogError("The pawn " + pawn.name + " has no PawnUI component.");
                    return;
                }

                PawnStatistics stat = pawn.GetComponent<PawnStatistics>();
                if(stat == null)
                {
                    Debug.LogError("The pawn " + pawn.name + " has no PawnStatistics component.");
                    return;
                }

                m_Behavior.m_UIPawnState[i].gameObject.SetActive(true);
                m_Behavior.m_UIPawnState[i].SetAvatar(pawnUIComponent.m_TurnSprite);
                m_Behavior.m_UIPawnState[i].SetHP(stat.m_HP, stat.m_MaxHP);
                m_Behavior.m_UIPawnState[i].SetMP(stat.m_MP, stat.m_MaxMP);
                m_Behavior.m_UIPawnState[i].SetName(stat.m_PawnName);
            }

            //hide unused state ui.
            int UIStateCount = m_Behavior.m_UIPawnState.Length;
            for(int i = playerPawnCount; i < UIStateCount; ++i)
            {
                m_Behavior.m_UIPawnState[i].gameObject.SetActive(false);
            }

            m_Runner.SetCurrentState((int)GameMasterState.RunSingleTurn, "Init state over");
        }
    }

    public class GameMaster_State_DummyLoop : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.DummyLoop; } }
    }
}
