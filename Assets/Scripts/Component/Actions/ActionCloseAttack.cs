using UnityEngine;
using Assets.Scripts.Assets;
using Assets.Scripts.Entities.Combat;

namespace Assets.Scripts.Component.Actions
{
    [System.Serializable]
    public class ActionCloseAttack : IAction
    {
        public string m_AnimTrigRunState;

        public string m_AnimTrigAttackState;

        public string m_AnimTrigIdleState;

        public float m_Speed;

        public float m_AttackDistance;

        private Vector3 m_InitialPosition;
        private Vector3 m_TargetPosition;
        private Vector3 m_AttackPosition;
        private float m_TravelTime;

        private float m_TimeStartTravel;

        private enum CloseUpAttackState
        {
            MoveToEnemy,
            Attack,
            ComeBack
        }
        private CloseUpAttackState m_State;

        public override void Prepare()
        {
            m_InitialPosition = m_Pawn.transform.position;
            m_TargetPosition = m_Target.transform.position;

            Vector3 direction = m_TargetPosition - m_InitialPosition;
            float distanceToTravel = direction.magnitude - m_AttackDistance;
            m_AttackPosition = m_InitialPosition + direction.normalized * distanceToTravel;

            m_TravelTime = distanceToTravel / m_Speed;

            m_State = CloseUpAttackState.MoveToEnemy;

            m_TimeStartTravel = Time.fixedTime;
        }

        public override Result Execute()
        {
            switch(m_State)
            { 
                case CloseUpAttackState.MoveToEnemy:
                    return Execute_MoveToEnemy();

                case CloseUpAttackState.Attack:
                    return Execute_Attack();

                case CloseUpAttackState.ComeBack:
                    return Execute_ComeBack();

                default:
                    Debug.LogError(string.Format("unknown state {0}", m_State));
                    break;
            }
            return Result.Continue;
        }

        public override void Resolve(ResolveResult _result)
        {
            PawnStatistics pawnStats = m_Pawn.GetComponent<PawnStatistics>();
            PawnStatistics targetStats = m_Target.GetComponent<PawnStatistics>();

            float weaponBonus = 0;
            if(pawnStats.m_EquippedWeapon != null)
            {
                Weapon wpn = pawnStats.m_EquippedWeapon;
                weaponBonus = wpn.m_Atk + Random.Range(-wpn.m_AtkR, wpn.m_AtkR);
            }

            float dmg = pawnStats.m_Atk + weaponBonus - targetStats.m_Def;
            if(dmg < 0) dmg = 0;
            int realDamage = (int)dmg;

            targetStats.m_HP -= realDamage;
            if (targetStats.m_HP <= 0)
            {
                targetStats.m_HP = 0;
                PawnBehavior behavior = m_Target.GetComponent<PawnBehavior>();
                behavior.m_State = PawnState.Dead;
            }

            _result.m_Damage = realDamage;
        }

        private Result Execute_MoveToEnemy()
        {
            float t = (Time.fixedTime - m_TimeStartTravel) / m_TravelTime;

            if (t < 1)
            {
                Vector3 newPos = Vector3.Lerp(m_InitialPosition, m_AttackPosition, t);
                m_Pawn.transform.position = newPos;
            }
            else
            {
                m_Pawn.transform.position = m_AttackPosition;
                m_State = CloseUpAttackState.Attack;
                //get anim graph and go to the next state.
            }
            StickToGround.Apply(m_Pawn);
            return Result.Continue;
        }

        private Result Execute_Attack()
        {
            //check if the anim state is over
            m_State = CloseUpAttackState.ComeBack;
            m_TimeStartTravel = Time.fixedTime;
            return Result.Continue;
        }

        private Result Execute_ComeBack()
        {
            float t = (Time.fixedTime - m_TimeStartTravel) / m_TravelTime;

            if (t < 1)
            {
                Vector3 newPos = Vector3.Lerp(m_AttackPosition, m_InitialPosition, t);
                m_Pawn.transform.position = newPos;
                StickToGround.Apply(m_Pawn);
                return Result.Continue;
            }
            else
            {
                m_Pawn.transform.position = m_InitialPosition;
                //get anim graph to the idle state
            }

            return Result.Over;
        }
    }
}
