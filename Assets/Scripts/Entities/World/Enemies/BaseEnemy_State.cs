using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AssemblyCSharp;
using UnityEngine;

using Persistent;
using Persistent.WorldEntity;

namespace Assets.Scripts.Entities.World.Enemies
{
    public partial class BaseEnemy_Behavior : IFreezableMonoBehavior
    {
        public class BaseEnemy_State_Birth : IFSMState<BaseEnemy_Behavior>
        {
            private float m_BirstStateStartTime;

            public override int State { get { return (int)EnemyState.Birth; } }

            public override void OnEnter()
            {
                m_BirstStateStartTime = Time.fixedTime;

                m_Behavior.m_BirthEffectInstance.SetActive(true);
                UnityEngine.Component[] components = m_Behavior.m_BirthEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));
                foreach (ParticleEmitter emitter in components)
                    emitter.emit = true;
            }

            public override void OnExecute()
            {
                if (Time.fixedTime >= m_BirstStateStartTime + m_Behavior.m_WorldSettings.m_BirthEffectDuration)
                {
                    UnityEngine.Component[] components = m_Behavior.m_BirthEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));

                    foreach (ParticleEmitter emitter in components)
                        emitter.emit = false;

                    m_Runner.SetCurrentState((int)EnemyState.Life, "Birth over");
                }
            }

            public override void OnExit()
            {
                m_Behavior.EnabledWorldMeshRenderer(true);
            }
        }

        public class BaseEnemy_State_Life : IFSMState<BaseEnemy_Behavior>
        {
            private int m_nextWaypointId;

            private Spawner_Behavior m_Spawner;

            private NavMeshAgent m_Agent;

            public override int State { get { return (int)EnemyState.Life; } }

            public override void Initialize()
            {
                m_Agent = m_Behavior.GetComponent<NavMeshAgent>();
                m_Spawner = m_Behavior.m_Spawner;
            }

            public override void OnEnter()
            {
                m_nextWaypointId = m_Spawner.GetNextWaypoint(-1);
            }

            public override void OnExecute()
            {
                Vector3 worldPos = m_Spawner.transform.TransformPoint(m_Spawner.m_waypoints[m_nextWaypointId]);
                m_Agent.SetDestination(worldPos);

                Vector3 direction = worldPos - m_GameObject.transform.position;
                if (direction.magnitude <= m_Behavior.m_WorldSettings.m_WaypointRadius)
                    m_nextWaypointId = m_Spawner.GetNextWaypoint(m_nextWaypointId);
            }

            public override void OnExit()
            {
                m_Agent.Stop(true);
            }
        }

        public class BaseEnemy_State_Death : IFSMState<BaseEnemy_Behavior>
        {
            private float m_StartTimeState;

            public override int State { get { return (int)EnemyState.DeathEffect; } }

            public override void OnEnter()
            {
                m_StartTimeState = Time.fixedTime;
                m_Behavior.m_DeathEffectInstance.SetActive(true);
                UnityEngine.Component[] components = m_Behavior.m_DeathEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));
                foreach (ParticleEmitter emitter in components)
                    emitter.emit = true;
            }

            public override void OnExecute()
            {
                if(Time.fixedTime >= m_StartTimeState + m_Behavior.m_WorldSettings.m_DeathEffectDuration)
                {
                    UnityEngine.Component[] components = m_Behavior.m_DeathEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));
                    foreach (ParticleEmitter emitter in components)
                        emitter.emit = false;

                    m_Behavior.EnabledWorldMeshRenderer(false);
                    m_Runner.SetCurrentState((int)EnemyState.WaitForEndOfDeathEffect, "timpe elapsed");
                }
            }
        }

        public class BaseEnemy_State_EndOfDeath : IFSMState<BaseEnemy_Behavior>
        {
            public override int State { get { return (int)EnemyState.WaitForEndOfDeathEffect; } }

            public override void OnExecute()
            {
                bool effectOver = true;

                UnityEngine.Component[] components = m_Behavior.m_DeathEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));
                foreach (ParticleEmitter emitter in components)
                {
                    if(emitter.particleCount != 0)
                    {
                        effectOver = false;
                        break;
                    }
                }

                if(effectOver)
                {
                    m_Behavior.m_Spawner.OnEntityDies(m_GameObject);
                    m_Behavior.m_DeathEffectInstance.SetActive(false);
                    m_GameObject.SetActive(false);
                }
                    
            }
        }

        public class BaseEnemy_State_Idle : IFSMState<BaseEnemy_Behavior>
        {
            public override int State
            {
                get { return (int)EnemyState.Idle; }
            }

            public override void OnEnter()
            {
                Animator anim = m_Behavior.GetComponent<Animator>();
                if (anim == null)
                    return;

                anim.SetTrigger(m_Behavior.m_TriggerAnimToPlay);
                m_Behavior.EnabledWorldMeshRenderer(true);
            }
        }
    }
}
