using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnBehavior : MonoBehaviour
    {
        public PawnState m_State;

        public string m_TriggerIdleState;

        public string m_TriggerDyingState;

        public string m_TriggerNothingState;

        private Vector3 m_InitialPosition;

        public void Start()
        {
            m_InitialPosition = gameObject.transform.position;
        }

        public void SetDeadState()
        {
            m_State = PawnState.Dead;
            TriggerAnimState(m_TriggerDyingState);
        }

        public void SetNothingState()
        {
            TriggerAnimState(m_TriggerNothingState);
        }

        public void SetIdleState()
        {
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
