using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AssemblyCSharp;
using UnityEngine;

namespace Persistent.WorldEntity
{
    public partial class BaseEnemy_Behavior : MonoBehaviour
    {
        public class BaseEnemy_State_Birth : IFSMState<BaseEnemy_Behavior>
        {
            private float m_BirstStateStartTime;

            public override int State { get { return (int)EnemyState.Birth; } }

            public override void OnEnter()
            {
                m_BirstStateStartTime = Time.fixedTime;
                
                Component[] components = m_Behavior.m_BirthEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));

                foreach (ParticleEmitter emitter in components)
                    emitter.emit = true;
            }

            public override void OnExecute()
            {
                if (Time.fixedTime >= m_BirstStateStartTime + m_Behavior.m_BirthEffectDuration)
                {
                    Component[] components = m_Behavior.m_BirthEffectInstance.GetComponentsInChildren(typeof(ParticleEmitter));

                    foreach (ParticleEmitter emitter in components)
                        emitter.emit = false;

                    m_Runner.SetCurrentState((int)EnemyState.Life, "Birth over");
                }
            }

            public override void OnExit()
            {
                m_Behavior.EnabledMeshRenderer(true);
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
                if (direction.magnitude <= m_Behavior.m_WaypointRadius)
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

            private GameObject m_deathEffect;

            public override int State { get { return (int)EnemyState.Death; } }

            public override void OnEnter()
            {
                m_StartTimeState = Time.fixedTime;
                Transform instance = (Transform)Instantiate(m_Behavior.m_DeathEffect, m_GameObject.transform.position, Quaternion.identity);
                m_deathEffect = instance.gameObject;
            }

            public override void OnExecute()
            {
                if(Time.fixedTime >= m_StartTimeState + m_Behavior.m_DeathEffectDuration)
                {
                    m_Behavior.m_Spawner.OnEntityDies(m_GameObject);
                    UnityEngine.Object.Destroy(m_deathEffect);
                    m_GameObject.SetActive(false);
                }
            }
        }
    }
}
