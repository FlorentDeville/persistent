using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Terminate : IFSMState<GameMaster>
    {
        public override int State { get { return (int)GameMasterState.Terminate; } }
    }
}
