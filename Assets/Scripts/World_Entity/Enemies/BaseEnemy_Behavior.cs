using UnityEngine;
using AssemblyCSharp;

using Assets.Scripts.Manager;

namespace Persistent.WorldEntity
{
    public partial class BaseEnemy_Behavior : MonoBehaviour
    {
        public float m_WaypointRadius;

        public Transform m_BirthEffect;

        public float m_BirthEffectDuration;

        public Transform m_DeathEffect;

        public float m_DeathEffectDuration;

        private Spawner_Behavior m_Spawner;

        private FSMRunner m_Runner;

        private GameObject m_BirthEffectInstance;

        private GameObject m_DeathEffectInstance;

        // Use this for initialization
        void Start()
        {
            m_Runner = new FSMRunner(gameObject);
            m_Runner.RegisterState<BaseEnemy_State_Birth>();
            m_Runner.RegisterState<BaseEnemy_State_Life>();
            m_Runner.RegisterState<BaseEnemy_State_Death>();
            m_Runner.RegisterState<BaseEnemy_State_EndOfDeath>();

            m_Runner.SetImmediateCurrentState((int)EnemyState.Birth);

            Transform instance = (Transform)Instantiate(m_BirthEffect, transform.position, Quaternion.identity);
            instance.parent = transform;
            m_BirthEffectInstance = instance.gameObject;
            m_BirthEffectInstance.SetActive(false);

            instance = (Transform)Instantiate(m_DeathEffect, transform.position, Quaternion.identity);
            instance.parent = transform;
            m_DeathEffectInstance = instance.gameObject;
            m_DeathEffectInstance.SetActive(false);
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

            EnabledMeshRenderer(false);
        }
        
        public void SetSpawner(Spawner_Behavior _spawner)
        {
            m_Spawner = _spawner;
        }

        private void EnabledMeshRenderer(bool _enabled)
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
                renderer.enabled = _enabled;
        }

        void OnTriggerEnter(Collider _col)
        {
            PlayerBehavior behavior = _col.GetComponent<PlayerBehavior>();
            if(behavior != null)
            {
                if(m_Runner.GetCurrentState() != (int)EnemyState.DeathEffect
                    && m_Runner.GetCurrentState() != (int)EnemyState.WaitForEndOfDeathEffect)
                {
                    m_Runner.SetCurrentState((int)EnemyState.DeathEffect, "collision with player");
                    GameSceneManager.GetInstance().LoadCombatScene("Level_01_Combat_01");
                }
            }
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