using Assets.Scripts.Component.Actions;

using UnityEngine;

#pragma warning disable 649

namespace Assets.Scripts.Component.MarkerEvent
{
    public class PawnParticleSystem : MonoBehaviour
    {
        public enum Mode
        {
            OnPawn,
            OnTarget,
            FromPawnToTarget,
            FromTargetToPawn
        }

        [SerializeField]
        [Tooltip("Prefab of a particle system to play")]
        Transform m_ParticleSystem;

        [SerializeField]
        Mode m_Mode;

        float m_LifeTime;
        float m_StartTime;

        bool m_Started;

        GameObject m_ObjParticleSystem;

        void Awake()
        {
            m_Started = false;
            m_ObjParticleSystem = null;
        }

        void Update()
        {
            if (!m_Started)
                return;

            if(m_StartTime + m_LifeTime <= Time.fixedTime)
            {
                StopEffect();
            }
        }

        public void StartEffect(float _lifetime)
        {
            if(m_ObjParticleSystem == null)
            {
                Transform instance = (Transform)Instantiate(m_ParticleSystem, Vector3.zero, Quaternion.identity);
                m_ObjParticleSystem = instance.gameObject;
            }

            m_LifeTime = _lifetime;
            m_StartTime = Time.fixedTime;

            switch(m_Mode)
            {
                case Mode.OnTarget:
                    StartOnTarget();
                    break;

                default:
                    break;
            }

            m_Started = true;
        }

        public void StopEffect()
        {
            StopParticleSystem();
            m_Started = false;
        }

        void StartOnTarget()
        {
            IAction action = GameMaster.GetInstance().GetSelectedAction();
            m_ObjParticleSystem.transform.parent = action.m_Target.transform;
            m_ObjParticleSystem.transform.localPosition = Vector3.zero;
            StartParticleSystem();
        }

        void StartParticleSystem()
        {
            m_ObjParticleSystem.SetActive(true);
            ParticleEmitter[] emitters = m_ObjParticleSystem.GetComponentsInChildren<ParticleEmitter>();
            foreach(ParticleEmitter emitter in emitters)
            {
                emitter.emit = true;
            }
        }

        void StopParticleSystem()
        {
            ParticleEmitter[] emitters = m_ObjParticleSystem.GetComponentsInChildren<ParticleEmitter>();
            foreach (ParticleEmitter emitter in emitters)
            {
                emitter.emit = false;
            }
        }

    }
}
