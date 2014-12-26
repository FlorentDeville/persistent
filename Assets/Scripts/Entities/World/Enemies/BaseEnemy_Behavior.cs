using UnityEngine;
using AssemblyCSharp;

using Assets.Scripts.Entities;
using Assets.Scripts.Manager;
using Assets.Scripts.Manager.Parameter;

using Persistent;
using Persistent.WorldEntity;

namespace Assets.Scripts.Entities.World.Enemies
{
    [RequireComponent(typeof(BaseEnemy_WorldSettings))]
    [RequireComponent(typeof(BaseEnemy_CombatSettings))]
    public partial class BaseEnemy_Behavior : IFreezableMonoBehavior
    {
        private BaseEnemy_WorldSettings m_WorldSettings;

        private BaseEnemy_CombatSettings m_CombatSettings;

        private Spawner_Behavior m_Spawner;

        private FSMRunner m_Runner;

        private GameObject m_BirthEffectInstance;

        private GameObject m_DeathEffectInstance;

        private bool m_CanEnterInCombat;

        // Use this for initialization
        void Start()
        {
            m_WorldSettings = GetComponent<BaseEnemy_WorldSettings>();
            m_CombatSettings = GetComponent<BaseEnemy_CombatSettings>();

            m_Runner = new FSMRunner(gameObject);
            m_Runner.RegisterState<BaseEnemy_State_Birth>();
            m_Runner.RegisterState<BaseEnemy_State_Life>();
            m_Runner.RegisterState<BaseEnemy_State_Death>();
            m_Runner.RegisterState<BaseEnemy_State_EndOfDeath>();

            Transform instance = (Transform)Instantiate(m_WorldSettings.m_BirthEffect, transform.position, Quaternion.identity);
            instance.parent = transform;
            m_BirthEffectInstance = instance.gameObject;
            m_BirthEffectInstance.SetActive(false);

            instance = (Transform)Instantiate(m_WorldSettings.m_DeathEffect, transform.position, Quaternion.identity);
            instance.parent = transform;
            m_DeathEffectInstance = instance.gameObject;
            m_DeathEffectInstance.SetActive(false);

            m_Runner.StartState((int)EnemyState.Birth);

            EnabledWorldMeshRenderer(false);
            EnableCombatMeshRenderer(false);

            m_CanEnterInCombat = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(!m_Freezed)
                m_Runner.Update();
        }

        public void OnSpawn()
        {
            m_CanEnterInCombat = true;
            if(m_Runner != null)
                m_Runner.StartState((int)EnemyState.Birth);

            EnabledWorldMeshRenderer(false);
        }
        
        public void SetSpawner(Spawner_Behavior _spawner)
        {
            m_Spawner = _spawner;
        }

        private void EnabledWorldMeshRenderer(bool _enabled)
        {
            m_WorldSettings.m_Gfx.SetActive(_enabled);
        }

        private void EnableCombatMeshRenderer(bool _enabled)
        {
            m_CombatSettings.m_Gfx.SetActive(_enabled);
        }

        void OnTriggerEnter(Collider _col)
        {
            PlayerBehavior behavior = _col.GetComponent<PlayerBehavior>();
            if(behavior != null)
            {
                if(m_CanEnterInCombat)
                {
                    m_CanEnterInCombat = false;

                    m_Runner.SetCurrentState((int)EnemyState.DeathEffect, "collision with player");

                    LevelMaster.GetInstance().StartBattleTransition(m_CombatSettings);
                }
            }
        }

        protected override void OnFreeze(bool _value)
        {
            GetComponent<NavMeshAgent>().enabled = !_value;
        }
    }

    public enum EnemyState
    {
        Undefined,
        Birth,
        Life,
        DeathEffect,
        WaitForEndOfDeathEffect,
        Count
    }
}