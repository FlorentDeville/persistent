using UnityEngine;
using System.Collections;

public class Combat_Settings : MonoBehaviour 
{
    [System.Serializable]
    public class InnerTransform
    {
        public Vector3 m_Position;
        public Vector3 m_Rotation;
    }

    public InnerTransform[] m_PawnsEnemyPosition;
    public InnerTransform[] m_PawnsPlayerPosition;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "icon_combat", false);

        if(m_PawnsEnemyPosition != null)
        {
            foreach (InnerTransform enemyTransform in m_PawnsEnemyPosition)
            {
                Vector3 worldPos = transform.TransformPoint(enemyTransform.m_Position);
                Gizmos.DrawIcon(worldPos, "icon_monster", false);
            }
                
        }

        if (m_PawnsPlayerPosition != null)
        {
            foreach (InnerTransform playerPawnTransform in m_PawnsPlayerPosition)
            {
                Vector3 worldPos = transform.TransformPoint(playerPawnTransform.m_Position);
                Gizmos.DrawIcon(worldPos, "icon_pacman", false);
            }

        }
    }

    public Vector3 ComputePawnEnemyGlobalPosition(int id)
    {
        return gameObject.transform.TransformPoint(m_PawnsEnemyPosition[id].m_Position);
    }

    public Quaternion ComputePawnEnemyGlobalOrientation(int id)
    {
        Quaternion localRotation = Quaternion.Euler(m_PawnsEnemyPosition[id].m_Rotation);
        return gameObject.transform.localRotation * localRotation;
    }

    public Vector3 ComputePawnPlayerGlobalPosition(int id)
    {
        return gameObject.transform.TransformPoint(m_PawnsPlayerPosition[id].m_Position);
    }

    public Quaternion ComputePawnPlayerGlobalOrientation(int id)
    {
        Quaternion localRotation = Quaternion.Euler(m_PawnsPlayerPosition[id].m_Rotation);
        return gameObject.transform.localRotation * localRotation;
    }
}
