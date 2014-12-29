using AssemblyCSharp;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities.Combat;
using UnityEngine;
using UnityEngine.UI;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Init : IFSMState<GameMaster>
    {
        public RawImage m_FadeWidget;
        public Color m_InitColor;

        public override int State { get { return (int)GameMasterState.Init; } }

        public override void OnEnter()
        {
            //compute turns history
            GameTurnManager turnMng = GameTurnManager.GetInstance();
            turnMng.Init(m_Behavior.m_Pawns);
            m_Behavior.m_UITurnHistory.UpdateTurnHistory(turnMng);

            //init ui state
            int playerPawnCount = turnMng.m_PlayerPawns.Count;
            for (int i = 0; i < playerPawnCount; ++i)
            {
                GameObject pawn = turnMng.m_PlayerPawns[i];
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

                InitializePawnStatistics(pawn);

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

            m_Behavior.m_ActionsMenu.Hide();

            m_FadeWidget.gameObject.SetActive(true);
            m_FadeWidget.color = m_InitColor;

            m_Runner.SetCurrentState((int)GameMasterState.PlayIntro, "Init state over");
        }

        private void InitializePawnStatistics(GameObject _pawn)
        {
            GameTurnManager turnMng = GameTurnManager.GetInstance();

            PawnBehavior bhv = _pawn.GetComponent<PawnBehavior>();
            Character savedCharacter = GameStateManager.GetInstance().GetCharacter(bhv.m_CharacterId);

            PawnStatistics stats = _pawn.GetComponent<PawnStatistics>();
            savedCharacter.LoadTo(stats);        
        }
    }

    public class GameMaster_State_DummyLoop : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.DummyLoop; } }
    }
}
