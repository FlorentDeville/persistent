using UnityEngine;
using AssemblyCSharp;

namespace Persistent.WorldEntity
{
    public partial class BaseEnemy_Behavior : MonoBehaviour
    {
        public float m_WaypointRadius;

        public Transform m_DeathEffect;

        public float m_DeathEffectDuration;

        private Spawner_Behavior m_Spawner;

        private NavMeshAgent m_Agent;

        private FSMRunner m_Runner;

        // Use this for initialization
        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();

            m_Runner = new FSMRunner(gameObject);
            m_Runner.RegisterState<BaseEnemy_State_Birth>();
            m_Runner.RegisterState<BaseEnemy_State_Life>();
            m_Runner.RegisterState<BaseEnemy_State_Death>();

            m_Runner.SetImmediateCurrentState((int)EnemyState.Birth);
        }

        // Update is called once per frame
        void Update()
        {
            m_Runner.Update();
        }

        public void OnSpawn()
        {
            if(m_Runner != null)
                m_Runner.StartState((int)EnemyState.Birth, "Spawn");
        }
        
        public void SetSpawner(Spawner_Behavior _spawner)
        {
            m_Spawner = _spawner;
        }

        void OnTriggerEnter(Collider _col)
        {
            PlayerBehavior behavior = _col.GetComponent<PlayerBehavior>();
            if(behavior != null)
            {
                m_Runner.SetCurrentState((int)EnemyState.Death, "Collision with player");
            }
        }
    }

    public enum EnemyState
    {
        Undefined,
        Birth,
        Life,
        Death,
        Count
    }
}