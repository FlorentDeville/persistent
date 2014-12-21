using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        class RunSingleTurn_State_Over : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.Over; } }
        }
    }
}
