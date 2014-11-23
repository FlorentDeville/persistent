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
        public class BaseEnemy_State_Birth : IFSMState
        {
            public BaseEnemy_State_Birth(FSMRunner _Runner, GameObject _gameObject)
            {
                m_Runner = _Runner;
                m_GameObject = _gameObject;
            }

            public override void OnEnter()
            {
                m_Runner.SetCurrentState((int)EnemyState.Life, "Birth over");
            }
        }

        public class BaseEnemy_State_Life : IFSMState
        {
            private BaseEnemy_Behavior m_Behavior;

            private int m_nextWaypointId;

            private Spawner_Behavior m_Spawner;

            private NavMeshAgent m_Agent;

            public BaseEnemy_State_Life(FSMRunner _Runner, GameObject _gameObject)
            {
                m_Runner = _Runner;
                m_GameObject = _gameObject;
                m_Behavior = m_GameObject.GetComponent<BaseEnemy_Behavior>();
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

        public class BaseEnemy_State_Death : IFSMState
        {
            private BaseEnemy_Behavior m_Behavior;

            private float m_StartTimeState;

            private GameObject m_deathEffect;

            public BaseEnemy_State_Death(FSMRunner _Runner, GameObject _GameObject)
            {
                m_Runner = _Runner;
                m_GameObject = _GameObject;
                m_Behavior = m_GameObject.GetComponent<BaseEnemy_Behavior>();
            }

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
