using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Init : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.Init; } }

        public override void OnEnter()
        {
            m_Behavior.m_TurnManager.Init();
        }
    }

    public class GameMaster_State_DummyLoop : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.DummyLoop; } }
    }
}
