using UnityEngine;
using System.Collections.Generic;

using Assets.Scripts.Manager.Parameter;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities.Combat;

using AssemblyCSharp;

[RequireComponent(typeof(Combat_Settings))]
public partial class GameMaster : MonoBehaviour 
{
    public CombatUI m_CombatUI;

    public enum GameMasterState
    {
        Init,
        RunSingleTurn,
        CheckGoal,
        GameOver,
        Victory,
        Terminate,
        DummyLoop
    }

    FSMRunner m_Runner;

    CombatSceneParameter m_SceneParameter;

    Combat_Settings m_Settings;

    List<GameObject> m_Pawns;

    GameTurnManager m_TurnManager;

	// Use this for initialization
	void Start () 
    {
        m_Runner = new FSMRunner(gameObject);
        m_Runner.RegisterState<GameMaster_State_Init>();
        m_Runner.RegisterState<GameMaster_State_RunSingleTurn>();
        m_Runner.RegisterState<GameMaster_State_CheckGoal>();
        m_Runner.RegisterState<GameMaster_State_GameOver>();
        m_Runner.RegisterState<GameMaster_State_Victory>();
        m_Runner.RegisterState<GameMaster_State_Terminate>();
        m_Runner.RegisterState<GameMaster_State_DummyLoop>();
        m_Runner.StartState((int)GameMasterState.Init);

        m_Pawns = new List<GameObject>();
        m_SceneParameter = GameSceneManager.GetInstance().GetParameter<CombatSceneParameter>();
        m_Settings = GetComponent<Combat_Settings>();

        for(int prefabId = 0; prefabId < m_SceneParameter.m_EnemiesPawns.Count; prefabId++)
        {
            Transform prefab = m_SceneParameter.m_EnemiesPawns[prefabId];

            Vector3 position = m_Settings.ComputePawnEnemyGlobalPosition(prefabId);
            Quaternion orientation = m_Settings.ComputePawnEnemyGlobalOrientation(prefabId);
            Transform pawn = (Transform)Instantiate(prefab, position, orientation);
            pawn.parent = gameObject.transform.root;
            m_Pawns.Add(pawn.gameObject);

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
            m_Pawns.Add(pawn.gameObject);

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

        m_TurnManager = new GameTurnManager(m_Pawns);
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_Runner.Update();
	}
}
