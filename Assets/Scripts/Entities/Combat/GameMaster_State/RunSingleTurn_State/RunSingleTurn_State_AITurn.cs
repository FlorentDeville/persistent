using AssemblyCSharp;
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
                m_Behavior.m_CanvasActions.gameObject.SetActive(false);
            }
        }
    }
}
