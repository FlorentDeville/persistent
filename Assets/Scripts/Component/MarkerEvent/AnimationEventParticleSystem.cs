using Assets.Scripts.Component.Actions;

using UnityEngine;

#pragma warning disable 649

namespace Assets.Scripts.Component.MarkerEvent
{
    public class AnimationEventParticleSystem : IAnimationEvent
    {
        [SerializeField]
        [Tooltip("Prefab of a particle system to play")]
        GameObject m_ParticleSystem;

        [SerializeField]
        DisplacementMode m_Mode;

        [SerializeField]
        [Tooltip("Duration of the particle system in second")]
        float m_LifeTime;

        float m_StartTime;

        bool m_Started;

        void Awake()
        {
            m_Started = false;
        }

        void Update()
        {
            if (!m_Started)
                return;

            if(m_StartTime + m_LifeTime <= Time.fixedTime)
            {
                StopEvent();
            }
        }

        public override void StartEvent()
        {
            m_StartTime = Time.fixedTime;

            switch(m_Mode)
            {
                case DisplacementMode.OnTarget:
                    StartOnTarget();
                    break;

                default:
                    break;
            }

            m_Started = true;
        }

        public override void StopEvent()
        {
            StopParticleSystem();
            m_Started = false;
        }

        void StartOnTarget()
        {
            IAction action = GameMaster.GetInstance().GetSelectedAction();
            transform.parent = action.m_Target.transform;
            transform.localPosition = Vector3.zero;
            StartParticleSystem();
        }

        void StartParticleSystem()
        {
            gameObject.SetActive(true);
            ParticleEmitter[] emitters = m_ParticleSystem.GetComponentsInChildren<ParticleEmitter>();
            foreach(ParticleEmitter emitter in emitters)
            {
                emitter.emit = true;
            }
        }

        void StopParticleSystem()
        {
            ParticleEmitter[] emitters = m_ParticleSystem.GetComponentsInChildren<ParticleEmitter>();
            foreach (ParticleEmitter emitter in emitters)
            {
                emitter.emit = false;
            }
        }

    }
}
