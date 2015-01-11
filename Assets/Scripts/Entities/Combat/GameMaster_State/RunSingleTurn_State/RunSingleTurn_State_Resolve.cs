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

            public override void Initialize()
            {
                base.Initialize();
            }

            public override void OnEnter()
            {
                //compute damage
                ActionRunner act = m_Behavior.GetRunSingleTurnState().GetSelectedAction();

                PawnStatistics src = act.m_Pawn.GetComponent<PawnStatistics>();
                PawnStatistics target = act.m_Target.GetComponent<PawnStatistics>();
                ResolveResult result = new ResolveResult();
                act.ActionDescription.m_Power.Resolve(src, target, result);

                if (act.ActionDescription.m_Power.m_DamageTarget == PawnTag.HP)
                {    
                    Vector3 effectPosition = Camera.main.WorldToScreenPoint(act.m_Target.transform.position);
                    m_Behavior.m_UIEffects.StartDamageEffect(effectPosition, result.m_Damage.ToString());
                }
                else
                    m_Runner.SetCurrentState((int)RunSingleTurnState.Over, "resolve over");
            }

            public override void OnExecute()
            {
                if(m_Behavior.m_UIEffects.IsDamageEffectOver())
                    m_Runner.SetCurrentState((int)RunSingleTurnState.Over, "resolve over");
            }

            public override void OnExit()
            {
                m_Behavior.m_UIEffects.HideDamageEffect();
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
