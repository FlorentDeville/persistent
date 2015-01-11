using AssemblyCSharp;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_Victory : IFSMState<GameMaster>
    {
        public float m_Duration;

        private float m_StartTime;

        public override int State { get { return (int)GameMasterState.Victory; } }

        public override void Initialize()
        {
            m_Duration = 2;
        }

        public override void OnEnter()
        {
            m_StartTime = Time.fixedTime;
            m_Behavior.m_UIEffects.ShowVictoryEffect();
        }

        public override void OnExecute()
        {
            if (m_StartTime + m_Duration < Time.fixedTime)
            {
                m_Behavior.m_UIEffects.HideVictoryEffect();
                m_Runner.SetCurrentState((int)GameMasterState.Terminate, "victory screen over");
            }
        }
    }
}
