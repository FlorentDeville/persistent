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

            m_Runner = new FSMRunner();

            BaseEnemy_State_Birth stateBirth = new BaseEnemy_State_Birth(m_Runner, gameObject);
            BaseEnemy_State_Life stateLife = new BaseEnemy_State_Life(m_Runner, gameObject);
            BaseEnemy_State_Death stateDeath = new BaseEnemy_State_Death(m_Runner, gameObject);

            m_Runner.AddState((int)EnemyState.Birth, stateBirth);
            m_Runner.AddState((int)EnemyState.Life, stateLife);
            m_Runner.AddState((int)EnemyState.Death, stateDeath);

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