using UnityEngine;
using System.Collections;

using Assets.Scripts.Manager.Parameter;
using Assets.Scripts.Manager;

[RequireComponent(typeof(Combat_Settings))]
public class GameMaster : MonoBehaviour 
{
    CombatSceneParameter m_SceneParameter;

    Combat_Settings m_Settings;

	// Use this for initialization
	void Start () 
    {
        m_SceneParameter = GameSceneManager.GetInstance().GetParameter<CombatSceneParameter>();
        m_Settings = GetComponent<Combat_Settings>();

        for(int prefabId = 0; prefabId < m_SceneParameter.m_EnemiesPawns.Count; prefabId++)
        {
            Transform prefab = m_SceneParameter.m_EnemiesPawns[prefabId];

            Vector3 position = m_Settings.ComputePawnEnemyGlobalPosition(prefabId);
            Quaternion orientation = m_Settings.ComputePawnEnemyGlobalOrientation(prefabId);
            Transform pawn = (Transform)Instantiate(prefab, position, orientation);
            pawn.parent = gameObject.transform.root;

            //move it on the ground
            if (pawn.gameObject.renderer)
            {
                Ray ToTheGround = new Ray(pawn.position, Vector3.down);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ToTheGround, out hit))
                {
                    Vector3 newPosition = pawn.position - (Vector3.down * hit.distance - pawn.gameObject.renderer.bounds.extents);
                    pawn.position = newPosition;
                }
            }
        }

        for(int prefabId = 0; prefabId < m_SceneParameter.m_PlayerPawns.Count; prefabId++)
        {
            Transform prefab = m_SceneParameter.m_PlayerPawns[prefabId];

            Vector3 position = m_Settings.ComputePawnPlayerGlobalPosition(prefabId);
            Quaternion orientation = m_Settings.ComputePawnPlayerGlobalOrientation(prefabId);
            Transform pawn = (Transform)Instantiate(prefab, position, orientation);
            pawn.parent = gameObject.transform.root;

            //move it on the ground
            if (pawn.gameObject.renderer)
            {
                Ray ToTheGround = new Ray(pawn.position, Vector3.down);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ToTheGround, out hit))
                {
                    Vector3 newPosition = pawn.position - (Vector3.down * hit.distance - pawn.gameObject.renderer.bounds.extents);
                    pawn.position = newPosition;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
