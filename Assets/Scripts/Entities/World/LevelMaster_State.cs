using AssemblyCSharp;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Manager;
using Assets.Scripts.Manager.Parameter;

using Persistent;

namespace Assets.Scripts.Entities.World
{
    public enum LevelMasterState
    {
        Nothing,

        ActivateBattleUI,
        FadeToBlack,
        LoadBattleScene,

        FadeInFromBattle,
        UnfreezeGameObjects
    }

    public partial class LevelMaster : MonoBehaviour
    {
        public class LevelMaster_StateNothing : IFSMState<LevelMaster>
        {
            public override int State
            {
                get { return (int)LevelMasterState.Nothing; }
            }
        }

        public class LevelMaster_StateActivateBattleUI : IFSMState<LevelMaster>
        {
            public Image m_Widget;
            public float m_Duration;
            public float m_TurnCount;
            public float m_ScaleStart;
            public float m_ScaleStop;

            private float m_StartTime;
            private float m_TotalAngle;

            public override int State
            {
                get { return (int)LevelMasterState.ActivateBattleUI; }
            }

            public override void OnEnter()
            {
                m_StartTime = Time.fixedTime;
                m_TotalAngle = 360 * m_TurnCount;
                m_Widget.gameObject.SetActive(true);
            }

            public override void OnExecute()
            {
                float t = (Time.fixedTime - m_StartTime) / m_Duration;
                if(t> 1)
                {
                    m_Runner.SetCurrentState((int)LevelMasterState.FadeToBlack, "animation over");
                    return;
                }

                float scale = Mathf.Lerp(m_ScaleStart, m_ScaleStop, t);
                float angle = Mathf.Lerp(0, m_TotalAngle, t);
                m_Widget.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                m_Widget.rectTransform.localScale = new Vector3(scale, scale, 1);
            }
        }

        public class LevelMaster_StateFadeToBlack : IFSMState<LevelMaster>
        {
            public RawImage m_Widget;
            public Color m_StartColor;
            public Color m_EndColor;
            public float m_Duration;
            public Image m_BattleWidget;

            private float m_StartTime;

            public override int State
            {
                get { return (int)LevelMasterState.FadeToBlack; }
            }

            public override void OnEnter()
            {
                m_StartTime = Time.fixedTime;
                m_Widget.gameObject.SetActive(true);
            }

            public override void OnExecute()
            {
                float t = (Time.fixedTime - m_StartTime) / m_Duration;
                if(t > 1)
                {
                    m_Runner.SetCurrentState((int)LevelMasterState.LoadBattleScene, "fade off over");
                    return;
                }

                Color currentColor = Color.Lerp(m_StartColor, m_EndColor, t);
                m_Widget.color = currentColor;
            }

            public override void OnExit()
            {
                m_BattleWidget.gameObject.SetActive(false);
            }
        }

        public class LevelMaster_StateLoadBattleScene : IFSMState<LevelMaster>
        {
            public override int State
            {
                get { return (int)LevelMasterState.LoadBattleScene; }
            }

            public override void OnEnter()
            {
                CombatSceneParameter param = new CombatSceneParameter();
                param.m_EnemiesPawns.AddRange(m_Behavior.m_EnemyCombatSettings.m_PawnsPrefab);

                Transform prefabPawnPlayer = Resources.Load<Transform>("Prefabs/Pawns/Pawn_Player");
                Transform prefabPawnSidekick = Resources.Load<Transform>("Prefabs/Pawns/Pawn_Sidekick");
                param.m_PlayerPawns.Add(prefabPawnPlayer);
                param.m_PlayerPawns.Add(prefabPawnSidekick);

                GameSceneManager.GetInstance().LoadCombatScene("Level_01_Combat_01", param);

                m_Runner.SetImmediateCurrentState((int)LevelMasterState.Nothing);
            }
        }

        public class LevelMaster_StateFadeInFromBattle : IFSMState<LevelMaster>
        {
            public RawImage m_Widget;
            public Color m_StartColor;
            public Color m_EndColor;
            public float m_Duration;

            private float m_StartTime;

            public override int State
            {
                get { return (int)LevelMasterState.FadeInFromBattle; }
            }

            public override void OnEnter()
            {
                m_StartTime = Time.fixedTime;
                m_Widget.gameObject.SetActive(true);
            }

            public override void OnExecute()
            {
                float t = (Time.fixedTime - m_StartTime) / m_Duration;
                if (t > 1)
                {
                    m_Runner.SetCurrentState((int)LevelMasterState.UnfreezeGameObjects, "fade in over");
                    return;
                }

                Color currentColor = Color.Lerp(m_StartColor, m_EndColor, t);
                m_Widget.color = currentColor;
            }

            public override void OnExit()
            {
                m_Widget.gameObject.SetActive(false);
            }
        }

        public class LevelMaster_StateUnfreezeGameobjects : IFSMState<LevelMaster>
        {
            public override int State
            {
                get { return (int)LevelMasterState.UnfreezeGameObjects; }
            }

            public override void OnEnter()
            {
                LevelMaster.GetInstance().UnfreezeAll();
                PlayerBehavior bhv = GameObjectHelper.getPlayer().GetComponent<PlayerBehavior>();
                bhv.m_InputEnabled = true;

                m_Runner.SetCurrentState((int)LevelMasterState.Nothing, "level unfrozen");
            }
        }
    }
}
