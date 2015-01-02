using Assets.Scripts.Assets.SpecificAction;
using Assets.Scripts.Component.Actions;

using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnBehavior : MonoBehaviour
    {
        public int m_CharacterId; //make it an enum or something!!

        public PawnState m_State;

        public AttackDescription m_AttackDescription;

        [Tooltip("Radius of a sphere containing the mesh. It is used to compute the displacement for an action so the action will never move a pawn into another one.")]
        public float m_AttackAnchorRadius;

        [Header("Animation State Idle")]
        public string m_TriggerIdleState;
        public string m_AnimStateNameIdle;

        [Header("Animation State Dying")]
        public string m_TriggerDyingState;
        public string m_AnimStateNameDying;

        [Header("Animation State Nothing")]
        public string m_TriggerNothingState;
        public string m_AnimStateNameNothing;

        [Header("Animation State Normal Hit")]
        public string m_TriggerNormalHitState;
        public string m_AnimStateNameNormalHit;

        [Header("Animation State Reaction Slow")]
        public string m_TriggerReactionToSlow;
        public string m_AnimStateReactionToSlow;

        private Vector3 m_InitialPosition;
        private Animator m_Mecanim;

        void Awake()
        {
            m_Mecanim = GetComponentInChildren<Animator>();
        }

        public void Start()
        {
            StickToGround.Apply(gameObject);
            m_InitialPosition = gameObject.transform.position;
            switch (m_State)
            {
                case PawnState.Idle:
                    SetIdleState();
                    break;

                case PawnState.Dead:
                    SetDeadState();
                    break;

                case PawnState.Nothing:
                    SetNothingState();
                    break;
            }
        }

        public void SetDeadState()
        {
            if (IsInState(m_AnimStateNameDying))
                return;

            m_State = PawnState.Dead;
            TriggerAnimState(m_TriggerDyingState);
        }

        public void SetNothingState()
        {
            if (IsInState(m_AnimStateNameNothing))
                return;

            m_State = PawnState.Nothing;
            TriggerAnimState(m_TriggerNothingState);
        }

        public void SetIdleState()
        {
            if (IsInState(m_AnimStateNameIdle))
                return;

            m_State = PawnState.Idle;
            TriggerAnimState(m_TriggerIdleState);
        }

        public void SetNormalHitState()
        {
            if (IsInState(m_AnimStateNameNormalHit))
                return;
            m_State = PawnState.NormalHit;
            TriggerAnimState(m_TriggerNormalHitState);
        }

        public void SetStateReactionSlow()
        {
            if (IsInState(m_AnimStateReactionToSlow))
                return;
            m_State = PawnState.NormalHit;
            TriggerAnimState(m_TriggerReactionToSlow);
        }

        private void TriggerAnimState(string _triggerName)
        {
            if (!string.IsNullOrEmpty(_triggerName))
            {
                if (m_Mecanim != null)
                    m_Mecanim.SetTrigger(_triggerName);
            }
        }

        void LateUpdate()
        {
            if (m_State == PawnState.Dead)
                gameObject.transform.position = m_InitialPosition;
        }

        bool IsInState(string _stateName)
        {
            if (m_Mecanim == null)
                return false;

            int stateHash = Animator.StringToHash(string.Format("Base Layer.{0}", _stateName));
            AnimatorStateInfo info = m_Mecanim.GetCurrentAnimatorStateInfo(0);
            if (info.nameHash == stateHash)
                return true;

            return false;
        }
    }

    public enum PawnState
    {
        Idle,
        Nothing,
        Dead,
        NormalHit
    }
}
