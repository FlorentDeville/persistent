using AssemblyCSharp;

using UnityEngine;
using UnityEngine.UI;

using Persistent;
using Persistent.WorldEntity;

namespace Assets.Scripts.Entities.World
{
    public partial class LevelMaster : MonoBehaviour
    {
#pragma warning disable 649

        #region Variable Battle UI Animation
        
        [Header("Battle UI Animation"), SerializeField]
        private Image m_Widget;

        [SerializeField]
        private float m_AnimationDuration;

        [SerializeField]
        private float m_AnimationTurnCount;

        [SerializeField]
        private float m_AnimationScaleStartValue;

        [SerializeField]
        private float m_AnimationScaleStopValue;
        
        #endregion

        #region Variable Fade Off

        [Header("Fade Off"), SerializeField]
        private RawImage m_FadeOffWidget;

        [SerializeField]
        private Color m_FadeOffStartColor;

        [SerializeField]
        private Color m_FadeOffEndColor;

        [SerializeField]
        private float m_FadeOffDuration;

        #endregion

        #region Variable Fade In From Battle

        [Header("Fade In From Battle"), SerializeField]
        private RawImage m_FadeInWidget;

        [SerializeField]
        private Color m_FadeInStartColor;

        [SerializeField]
        private Color m_FadeInEndColor;

        [SerializeField]
        private float m_FadeInDuration;

        #endregion

        private static LevelMaster m_Instance = null;

        private FSMRunner m_Runner;

        private BaseEnemy_CombatSettings m_EnemyCombatSettings;

        public static LevelMaster GetInstance()
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<LevelMaster>();

            return m_Instance;
        }

        void Start()
        {
            m_Runner = new FSMRunner(gameObject);
            m_Runner.RegisterState<LevelMaster_StateNothing>();
            m_Runner.RegisterState<LevelMaster_StateLoadBattleScene>();
            m_Runner.RegisterState<LevelMaster_StateUnfreezeGameobjects>();

            LevelMaster_StateFadeInFromBattle stateFadeIn = m_Runner.RegisterState<LevelMaster_StateFadeInFromBattle>();
            stateFadeIn.m_Widget = m_FadeInWidget;
            stateFadeIn.m_StartColor = m_FadeInStartColor;
            stateFadeIn.m_EndColor = m_FadeInEndColor;
            stateFadeIn.m_Duration = m_FadeInDuration;

            LevelMaster_StateActivateBattleUI stateBattleUI = m_Runner.RegisterState<LevelMaster_StateActivateBattleUI>();
            stateBattleUI.m_Widget = m_Widget;
            stateBattleUI.m_TurnCount = m_AnimationTurnCount;
            stateBattleUI.m_Duration = m_AnimationDuration;
            stateBattleUI.m_ScaleStart = m_AnimationScaleStartValue;
            stateBattleUI.m_ScaleStop = m_AnimationScaleStopValue;
            m_Widget.gameObject.SetActive(false);

            LevelMaster_StateFadeToBlack stateFadeOff = m_Runner.RegisterState<LevelMaster_StateFadeToBlack>();
            stateFadeOff.m_Widget = m_FadeOffWidget;
            stateFadeOff.m_StartColor = m_FadeOffStartColor;
            stateFadeOff.m_EndColor = m_FadeOffEndColor;
            stateFadeOff.m_Duration = m_FadeOffDuration;
            stateFadeOff.m_BattleWidget = m_Widget;

            m_FadeOffWidget.gameObject.SetActive(false);

            m_Runner.StartState((int)LevelMasterState.Nothing);
        }

        void Update()
        {
            m_Runner.Update();
            //state nothing

            //state start battle
                //state activate UI Battle
                //state fade to black
                //state deactive UI battle
                //state prepare and load battle scene
        }

        public void FreezeAll()
        {
            IFreezableMonoBehavior[] allFreezable = FindObjectsOfType<IFreezableMonoBehavior>();
            foreach (IFreezableMonoBehavior obj in allFreezable)
                obj.Freezed = true;
        }

        public void UnfreezeAll()
        {
            IFreezableMonoBehavior[] allFreezable = FindObjectsOfType<IFreezableMonoBehavior>();
            foreach (IFreezableMonoBehavior obj in allFreezable)
                obj.Freezed = false;
        }

        public void StartBattleTransition(BaseEnemy_CombatSettings _combatSettings)
        {
            LevelMaster.GetInstance().FreezeAll();
            PlayerBehavior bhv = GameObjectHelper.getPlayer().GetComponent<PlayerBehavior>();
            bhv.m_InputEnabled = false;

            m_EnemyCombatSettings = _combatSettings;
            m_Runner.SetCurrentState((int)LevelMasterState.ActivateBattleUI, "transition requested");
        }

        public void ComeBackFromBattle()
        {
            m_Runner.SetCurrentState((int)LevelMasterState.FadeInFromBattle, "battle over");
        }
    }
}
