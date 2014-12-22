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
            WaitForAttackToStart,
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
            Result res = Result.Continue;
            switch(m_State)
            { 
                case CloseUpAttackState.MoveToEnemy:
                    res = Execute_MoveToEnemy();
                    break;

                case CloseUpAttackState.WaitForAttackToStart:
                    res = Execute_WaitForAttackToStart();
                    break;

                case CloseUpAttackState.Attack:
                    res = Execute_Attack();
                    break;

                case CloseUpAttackState.ComeBack:
                    res = Execute_ComeBack();
                    break;

                default:
                    Debug.LogError(string.Format("unknown state {0}", m_State));
                    break;
            }
            StickToGround.Apply(m_Pawn);
            return res;
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
                behavior.SetDeadState();
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
                Animator anim = m_Pawn.GetComponent<Animator>();
                if(anim != null)
                {
                    anim.SetTrigger(m_AnimTrigAttackState);
                }

                m_State = CloseUpAttackState.WaitForAttackToStart;
                //get anim graph and go to the next state.
            }
            return Result.Continue;
        }

        private Result Execute_Attack()
        {
            //check if the anim state is over
            m_Pawn.transform.position = m_AttackPosition;

            Animator anim = m_Pawn.GetComponent<Animator>();
            if (anim != null)
            {
                int AttackHash = Animator.StringToHash("Base Layer.Attack");
                int IdleHash = Animator.StringToHash("Base Layer.Idle");
                int NothingHash = Animator.StringToHash("Base Layer.Nothing");
                int DeadHash = Animator.StringToHash("Base Layer.Dead");
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if (info.nameHash != AttackHash)
                {
                    m_State = CloseUpAttackState.ComeBack;
                    m_TimeStartTravel = Time.fixedTime;  
                }
            }
            else
            {
                m_State = CloseUpAttackState.ComeBack;
                m_TimeStartTravel = Time.fixedTime;
            }
            return Result.Continue;
        }

        private Result Execute_ComeBack()
        {
            float t = (Time.fixedTime - m_TimeStartTravel) / m_TravelTime;

            if (t < 1)
            {
                Vector3 newPos = Vector3.Lerp(m_AttackPosition, m_InitialPosition, t);
                m_Pawn.transform.position = newPos;
                return Result.Continue;
            }
            else
            {
                m_Pawn.transform.position = m_InitialPosition;
                //get anim graph to the idle state
            }

            return Result.Over;
        }

        private Result Execute_WaitForAttackToStart()
        {
            m_Pawn.transform.position = m_AttackPosition;
            Animator anim = m_Pawn.GetComponent<Animator>();
            if (anim != null)
            {
                int AttackHash = Animator.StringToHash("Base Layer.Attack");
                int IdleHash = Animator.StringToHash("Base Layer.Idle");
                int NothingHash = Animator.StringToHash("Base Layer.Nothing");
                int DeadHash = Animator.StringToHash("Base Layer.Dead");
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if (info.nameHash == AttackHash)
                {
                    m_State = CloseUpAttackState.Attack;
                }
            }
            else
            {
                m_State = CloseUpAttackState.Attack;
            }

            return Result.Continue;
        }
    }
}
