using AssemblyCSharp;
using Assets.Scripts.Component.Actions;
using Assets.Scripts.Manager;
using UnityEngine;

public partial class GameMaster : MonoBehaviour
{
    public partial class GameMaster_State_RunSingleTurn : IFSMState<GameMaster>
    {
        public class RunSingleTurn_State_Resolve : IFSMState<GameMaster>
        {
            public override int State { get { return (int)RunSingleTurnState.Resolve; } }

            public float m_UIAnimationDuration;

            public float m_UIAnimationDisplacement;

            private float m_UIAnimationStartTime;

            private Vector3 m_UIAnimationInitialPosition;
            private Vector3 m_UIAnimationFinalPosition;

            public override void Initialize()
            {
                m_UIAnimationDuration = 1;
                m_UIAnimationDisplacement = 20;
                base.Initialize();
            }

            public override void OnEnter()
            {
                //compute damage
                IAction act = m_Behavior.GetRunSingleTurnState().GetSelectedAction();
                ResolveResult result = new ResolveResult();
                act.Resolve(result);

                //write damage in UI widget
                UnityEngine.UI.Text UIText = m_Behavior.m_UIDamageText.GetComponent<UnityEngine.UI.Text>();
                UIText.text = result.m_Damage.ToString();

                //Set position of ui widget
                m_UIAnimationInitialPosition = Camera.main.WorldToScreenPoint(act.m_Target.transform.position);
                m_Behavior.m_UIDamageText.transform.position = m_UIAnimationInitialPosition;

                //Activate ui widget
                m_Behavior.m_UIDamageText.SetActive(true);

                //initialize animation variables
                m_UIAnimationStartTime = Time.fixedTime;
                m_UIAnimationFinalPosition = m_UIAnimationInitialPosition + Vector3.up * m_UIAnimationDisplacement;
            }

            public override void OnExecute()
            {
                //render hit
                float t = (Time.fixedTime - m_UIAnimationStartTime) / m_UIAnimationDuration;

                if (t >= 1)
                {
                    m_Runner.SetCurrentState((int)RunSingleTurnState.Over, "resolve over");
                    return;
                }

                m_Behavior.m_UIDamageText.transform.position = Vector3.Lerp(m_UIAnimationInitialPosition, m_UIAnimationFinalPosition, t);
            }

            public override void OnExit()
            {
                m_Behavior.m_UIDamageText.SetActive(false);
                UpdateUIState();
            }

            private void UpdateUIState()
            {
                GameTurnManager turnMng = GameTurnManager.GetInstance();
                int playerPawnCount = turnMng.m_PlayerPawns.Count;
                for (int i = 0; i < playerPawnCount; ++i)
                {
                    GameObject pawn = turnMng.m_PlayerPawns[i];
                    PawnUI pawnUIComponent = pawn.GetComponent<PawnUI>();
                    if (pawnUIComponent == null)
                    {
                        Debug.LogError("The pawn " + pawn.name + " has no PawnUI component.");
                        return;
                    }

                    PawnStatistics stat = pawn.GetComponent<PawnStatistics>();
                    if (stat == null)
                    {
                        Debug.LogError("The pawn " + pawn.name + " has no PawnStatistics component.");
                        return;
                    }

                    m_Behavior.m_UIPawnState[i].SetHP(stat.m_HP, stat.m_MaxHP);
                    m_Behavior.m_UIPawnState[i].SetMP(stat.m_MP, stat.m_MaxMP);
                }
            }
        }
    }
}
