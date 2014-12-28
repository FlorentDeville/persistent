using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnBehavior : MonoBehaviour
    {
        public int m_CharacterId; //make it an enum or something!!

        public PawnState m_State;

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

        private Vector3 m_InitialPosition;

        public void Start()
        {
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

        private void TriggerAnimState(string _triggerName)
        {
            if (!string.IsNullOrEmpty(_triggerName))
            {
                Animator anim = GetComponent<Animator>();
                if (anim != null)
                    anim.SetTrigger(_triggerName);
            }
        }

        void LateUpdate()
        {
            if (m_State == PawnState.Dead)
                gameObject.transform.position = m_InitialPosition;
        }

        bool IsInState(string _stateName)
        {
            Animator anim = GetComponent<Animator>();
            if (anim == null)
                return false;

            int stateHash = Animator.StringToHash(string.Format("Base Layer.{0}", _stateName));
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
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
    }
}
