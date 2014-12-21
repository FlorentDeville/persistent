using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_RunAction : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.RunAction; } }

            public override void OnEnter()
            {
                
            }
        }
    }
}
