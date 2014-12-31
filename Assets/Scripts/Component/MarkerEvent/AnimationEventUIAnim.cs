using Assets.Scripts.Component.Actions;

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Component.MarkerEvent
{
    public class AnimationEventUIAnim : IAnimationEvent
    {
        [SerializeField]
        [Tooltip("A gameobject container to move the animation at the proper place. It must not be animated.")]
        private GameObject m_Container;

        [SerializeField]
        private string m_AnimationTrigger;

        [SerializeField]
        [Tooltip("Offset on the position of the content of the canvas in world coordinates")]
        private Vector3 m_OffsetPosition;

        [SerializeField]
        private float m_Duration;

        private float m_StartTime;

        private bool m_AnimationStarted;

        void Awake()
        {
            m_AnimationStarted = false;
            transform.SetParent(FindObjectOfType<SceneRoot_Behavior>().gameObject.transform);
        }

        void Update()
        {
            if (!m_AnimationStarted)
                return;

            if (Time.fixedTime > m_StartTime + m_Duration)
                StopEvent();
        }

        public override void StartEvent()
        {
            m_StartTime = Time.fixedTime;

            gameObject.SetActive(true);
            Animator anim = GetComponentInChildren<Animator>();
            anim.SetTrigger(m_AnimationTrigger);

            m_AnimationStarted = true;

            StartOnTarget();
        }

        public override void StopEvent()
        {
            m_Container.transform.position = Vector3.zero;
            m_AnimationStarted = false;
            gameObject.SetActive(false);
        }

        private void StartOnTarget()
        {
            IAction action = GameMaster.GetInstance().GetSelectedAction();

            Vector3 initialPosition = action.m_Target.transform.position;
            Vector3 finalPosition = initialPosition + m_OffsetPosition;
            
            Vector3 screenSpacePosition = Camera.main.WorldToScreenPoint(finalPosition);
            m_Container.transform.position = screenSpacePosition;
        }
    }
}
