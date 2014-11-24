using UnityEngine;
using System.Collections;

namespace Persistent.WorldEntity
{
    public class Spawner_Behavior : MonoBehaviour
    {
        public Vector3[] m_portals;

        public Vector3[] m_waypoints;

        public DisplacementMode m_mode;

        public Transform m_enemyPrefab;

        public float m_DelayBeforeSpawn;

        private GameObject[] m_enemies;

        private float[] m_DelayTimeStart;

        // Use this for initialization
        void Start()
        {
            m_enemies = new GameObject[m_portals.Length];
            m_DelayTimeStart = new float[m_portals.Length];
            for (int i = 0; i < m_portals.Length; ++i)
            {
                m_enemies[i] = Instantiate(m_portals[i]);
                Spawn(i);
                m_DelayTimeStart[i] = -1;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < m_enemies.Length; ++i)
            {
                if(m_DelayTimeStart[i] <= 0)
                    continue;

                if(Time.fixedTime >= m_DelayBeforeSpawn + m_DelayTimeStart[i])
                {
                    Spawn(i);
                    m_DelayTimeStart[i] = -1;
                }
                
            }
            
        }

        public int GetNextWaypoint(int _currentWaypoint)
        {
            switch(m_mode)
            {
                case DisplacementMode.Path:
                    return (_currentWaypoint + 1) % m_waypoints.Length;

                case DisplacementMode.Random:
                    return (int)((Random.value * 1000) % m_waypoints.Length);
            }

            return 0;
        }

        public void OnEntityDies(GameObject _deadEntity)
        {
            for (int i = 0; i < m_enemies.Length; ++i)
            {
                if (m_enemies[i] == _deadEntity)
                {
                    m_DelayTimeStart[i] = Time.fixedTime;
                    return;
                }
            }
        }

        void Spawn(int id)
        {
            m_enemies[id].SetActive(true);
            m_enemies[id].GetComponent<BaseEnemy_Behavior>().OnSpawn();
            m_enemies[id].transform.position = transform.TransformPoint(m_portals[id]);
        }

        GameObject Instantiate(Vector3 _portal)
        {
            Vector3 worldPos = transform.TransformPoint(_portal);
            Transform instance = (Transform)Instantiate(m_enemyPrefab, worldPos, Quaternion.identity);
            BaseEnemy_Behavior enemy = instance.gameObject.GetComponent<BaseEnemy_Behavior>();
            enemy.SetSpawner(this);

            return instance.gameObject;
        }

        void OnDrawGizmosSelected()
        {
            if (m_portals != null)
            {
                foreach (Vector3 portal in m_portals)
                {
                    Vector3 worldPos = transform.TransformPoint(portal);
                    Gizmos.DrawIcon(worldPos, "icon_portal");
                }
            }

            if (m_waypoints != null)
            {
                for (int i = 0; i < m_waypoints.Length; ++i)
                {
                    Vector3 waypoint = m_waypoints[i];
                    Vector3 nextWaypoint = m_waypoints[(i + 1) % m_waypoints.Length];

                    Vector3 worldPos = transform.TransformPoint(waypoint);
                    Gizmos.DrawIcon(worldPos, "icon_waypoint");

                    GizmosExtension.DrawConnectedLine(worldPos, transform.TransformPoint(nextWaypoint));
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "icon_spawner", false);
        }
    }

    public enum DisplacementMode
    {
        Undefined,
        Random,
        Path,
        Count
    }
}
