using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public enum RunSingleTurnState
        {
            Prepare,
            PlayerTurn,
            AITurn,
            RunAction,
            Resolve
        }

        public override int State { get { return (int)GameMasterState.RunSingleTurn; } }
    }
}
