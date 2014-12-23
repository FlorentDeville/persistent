using AssemblyCSharp;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities.Combat;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Terminate : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.Terminate; } }

        public override void OnEnter()
        {
            foreach(GameObject obj in GameTurnManager.GetInstance().m_PlayerPawns)
            { 
                PawnBehavior bhv = obj.GetComponent<PawnBehavior>();
                PawnStatistics stats = obj.GetComponent<PawnStatistics>();
                GameStateManager.GetInstance().GetCharacter(bhv.m_CharacterId).SaveFrom(stats);
            }

            GameSceneManager.GetInstance().Pop(true);
        }
    }
}
