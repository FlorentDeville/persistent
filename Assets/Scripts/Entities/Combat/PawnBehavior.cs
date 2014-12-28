using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnBehavior : MonoBehaviour
    {
        public int m_CharacterId; //make it an enum or something!!

        public PawnState m_State;

        public string m_TriggerIdleState;

        public string m_TriggerDyingState;

        public string m_TriggerNothingState;

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
            m_State = PawnState.Dead;
            TriggerAnimState(m_TriggerDyingState);
        }

        public void SetNothingState()
        {
            m_State = PawnState.Nothing;
            TriggerAnimState(m_TriggerNothingState);
        }

        public void SetIdleState()
        {
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
    }

    public enum PawnState
    {
        Idle,
        Nothing,
        Dead,
    }
}
