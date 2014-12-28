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

        private Quaternion m_InitialOrientation;
        private Vector3 m_InitialPosition;
        private Vector3 m_TargetPosition;
        private Vector3 m_AttackPosition;
        private float m_TravelTime;

        private float m_TimeStartTravel;

        private enum CloseUpAttackState
        {
            StartMoveToEnemy,
            MoveToEnemy,
            WaitForAttackToStart,
            Attack,
            StartComeBack,
            ComeBack
        }
        private CloseUpAttackState m_State;

        public override void Prepare()
        {
            m_InitialOrientation = m_Pawn.transform.localRotation;
            m_InitialPosition = m_Pawn.transform.position;
            m_TargetPosition = m_Target.transform.position;

            Vector3 direction = m_TargetPosition - m_InitialPosition;

            PawnBehavior pawnBhv = m_Pawn.GetComponent<PawnBehavior>();
            PawnBehavior targetBhv = m_Target.GetComponent<PawnBehavior>();
            float distanceToTravel = direction.magnitude - m_AttackDistance - pawnBhv.m_AttackAnchorRadius - targetBhv.m_AttackAnchorRadius;
            m_AttackPosition = m_InitialPosition + direction.normalized * distanceToTravel;

            m_TravelTime = distanceToTravel / m_Speed;

            m_State = CloseUpAttackState.StartMoveToEnemy;

            m_TimeStartTravel = Time.fixedTime;
        }

        public override Result Execute()
        {
            Result res = Result.Continue;
            switch(m_State)
            { 
                case CloseUpAttackState.StartMoveToEnemy:
                    res = Execute_StartMoveToEnemy();
                    break;

                case CloseUpAttackState.MoveToEnemy:
                    res = Execute_MoveToEnemy();
                    break;

                case CloseUpAttackState.WaitForAttackToStart:
                    res = Execute_WaitForAttackToStart();
                    break;

                case CloseUpAttackState.Attack:
                    res = Execute_Attack();
                    break;

                case CloseUpAttackState.StartComeBack:
                    res = Execute_StartComeBack();
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

        private Result Execute_StartMoveToEnemy()
        {
            if(!string.IsNullOrEmpty(m_AnimTrigRunState))
            {
                Animator anim = m_Pawn.GetComponentInChildren<Animator>();
                if(anim != null)
                {
                    anim.SetTrigger(m_AnimTrigRunState);
                }
            }

            m_State = CloseUpAttackState.MoveToEnemy;
            return Result.Continue;
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
                m_State = CloseUpAttackState.Attack;
                m_Pawn.transform.position = m_AttackPosition;
                Animator anim = m_Pawn.GetComponentInChildren<Animator>();
                if (anim != null && !string.IsNullOrEmpty(m_AnimTrigAttackState))
                {
                    anim.SetTrigger(m_AnimTrigAttackState);
                    m_State = CloseUpAttackState.WaitForAttackToStart;
                }
                //get anim graph and go to the next state.
            }

            m_Pawn.transform.forward = (m_AttackPosition - m_InitialPosition).normalized;
            return Result.Continue;
        }

        private Result Execute_WaitForAttackToStart()
        {
            m_Pawn.transform.position = m_AttackPosition;
            Animator anim = m_Pawn.GetComponentInChildren<Animator>();
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
            m_Pawn.transform.forward = (m_AttackPosition - m_InitialPosition).normalized;
            return Result.Continue;
        }

        private Result Execute_Attack()
        {
            //check if the anim state is over
            m_Pawn.transform.position = m_AttackPosition;

            Animator anim = m_Pawn.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                int AttackHash = Animator.StringToHash("Base Layer.Attack");
                int IdleHash = Animator.StringToHash("Base Layer.Idle");
                int NothingHash = Animator.StringToHash("Base Layer.Nothing");
                int DeadHash = Animator.StringToHash("Base Layer.Dead");
                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if (info.nameHash != AttackHash)
                {
                    m_State = CloseUpAttackState.StartComeBack;
                    m_TimeStartTravel = Time.fixedTime;
                    Enemy_NormalHit();
                }
            }
            else
            {
                m_State = CloseUpAttackState.StartComeBack;
                m_TimeStartTravel = Time.fixedTime;
                Enemy_NormalHit();
            }

            m_Pawn.transform.forward = (m_AttackPosition - m_InitialPosition).normalized;
            return Result.Continue;
        }

        private Result Execute_StartComeBack()
        {
            m_Pawn.transform.position = m_AttackPosition;
            if (!string.IsNullOrEmpty(m_AnimTrigRunState))
            {
                Animator anim = m_Pawn.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger(m_AnimTrigRunState);
                }
            }

            m_State = CloseUpAttackState.ComeBack;
            return Result.Continue;
        }

        private Result Execute_ComeBack()
        {
            float t = (Time.fixedTime - m_TimeStartTravel) / m_TravelTime;

            m_Pawn.transform.forward = (m_InitialPosition - m_AttackPosition).normalized;
            if (t < 1)
            {
                Vector3 newPos = Vector3.Lerp(m_AttackPosition, m_InitialPosition, t);
                m_Pawn.transform.position = newPos;
                return Result.Continue;
            }
            else
            {
                m_Pawn.transform.position = m_InitialPosition;
                m_Pawn.transform.localRotation = m_InitialOrientation;
            }

            if (!string.IsNullOrEmpty(m_AnimTrigIdleState))
            {
                Animator anim = m_Pawn.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    anim.ResetTrigger(m_AnimTrigRunState);
                    anim.SetTrigger(m_AnimTrigIdleState);
                }
            }
            return Result.Over;
        }

        private void Enemy_NormalHit()
        {
            m_Target.GetComponent<PawnBehavior>().SetNormalHitState();
        }
    }
}
