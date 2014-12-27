using AssemblyCSharp;
using UnityEngine;
using CinemaDirector;

public partial class GameMaster : MonoBehaviour
{
    public class GameMaster_State_PlayIntro : IFSMState<GameMaster>
    {
        public Cutscene m_IntroCutscene;

        private float m_StartTime;

        public override int State
        {
            get { return (int)GameMasterState.PlayIntro; }
        }

        public override void OnEnter()
        {
            m_StartTime = Time.fixedTime;
            m_IntroCutscene.Play();
        }

        public override void OnExecute()
        {
            if (Time.fixedTime >= m_StartTime + m_Behavior.m_ColorScreenDuration)
                m_Behavior.m_InitFadeWidget.gameObject.SetActive(false);

            if (m_IntroCutscene.State != Cutscene.CutsceneState.Playing)
                m_Runner.SetCurrentState((int)GameMasterState.RunSingleTurn, "intro over");
        }
    }
}
