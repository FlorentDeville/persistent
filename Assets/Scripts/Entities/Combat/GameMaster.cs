using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Assets.Scripts.Manager.Parameter;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities.Combat;

using AssemblyCSharp;
using Assets.Scripts.Entities.UI;
using Assets.Scripts.Component.Actions;

[RequireComponent(typeof(Combat_Settings))]
public partial class GameMaster : MonoBehaviour 
{
    public UnityEngine.EventSystems.EventSystem m_EventSystem;

    public CombatUI_TurnHistory m_UITurnHistory;

    public CombatUI_PawnState[] m_UIPawnState;

    public CombatUI_CanvasActions m_ActionsMenu;

    public Button m_AttackButton;
    public Button m_MagicButton;
    public Button m_ItemsButton;

    public CancelButton[] m_UIAttackEnemyButtons;

    public GameObject m_Cursor;

    //public Canvas m_CanvasActions;

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

        HideAllAttackEnemiesButtons();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_Runner.Update();
	}

    void OnAttackClicked()
    {
        int enemyCount = m_TurnManager.m_EnemiesPawns.Count;
        for(int i = 0; i < enemyCount; ++i)
        {
            GameObject obj = m_TurnManager.m_EnemiesPawns[i];

            CancelButton btn = m_UIAttackEnemyButtons[i];
            btn.gameObject.SetActive(true);
            Text txt = btn.transform.GetComponentInChildren<Text>();
            txt.text = m_TurnManager.m_EnemiesPawns[i].GetComponent<PawnStatistics>().m_PawnName;
            btn.onClick.AddListener(() =>
                {
                    Debug.Log(string.Format("select enemy {0}", txt.text));
                    HideAllAttackEnemiesButtons();

                    IAction attackAction = m_TurnManager.GetCurrentPawn().GetComponent<PawnActions>().GetAttackAction();
                    attackAction.SetTarget(obj);
                    SetSelectedAction(attackAction);

                });

            btn.onCancel.AddListener(() =>
                {
                    HideAllAttackEnemiesButtons();
                    m_AttackButton.Select();
                    m_Cursor.SetActive(false);
                    m_MagicButton.gameObject.SetActive(true);
                    m_ItemsButton.gameObject.SetActive(true);
                });

            btn.onSelect.AddListener(() =>
                {
                    Vector3 camPosition = Camera.main.WorldToScreenPoint(obj.transform.position);
                    m_Cursor.transform.position = camPosition;
                });
        }

        for(int i = enemyCount; i < m_UIAttackEnemyButtons.Length; ++i)
        {
            Button btn = m_UIAttackEnemyButtons[i];
            btn.gameObject.SetActive(false);
        }

        m_UIAttackEnemyButtons[0].Select();
        m_Cursor.SetActive(true);
        m_MagicButton.gameObject.SetActive(false);
        m_ItemsButton.gameObject.SetActive(false);
    }

    void HideAllAttackEnemiesButtons()
    {
        foreach (Button btn in m_UIAttackEnemyButtons)
            btn.gameObject.SetActive(false);
    }

    void SetSelectedAction(IAction _act)
    {
        GameMaster_State_RunSingleTurn runSingleTurnState = GetRunSingleTurnState();//m_Runner.GetStateObject<GameMaster_State_RunSingleTurn>((int)GameMasterState.RunSingleTurn);
        runSingleTurnState.SetSelectedAction(_act);
        //GameMaster_State_RunSingleTurn.RunSingleTurn_State_PlayerTurn playerTurn = runSingleTurnState.GetStateObject<GameMaster_State_RunSingleTurn.RunSingleTurn_State_PlayerTurn>(GameMaster.GameMaster_State_RunSingleTurn.RunSingleTurnState.PlayerTurn);
        //playerTurn.SetSelectedAction(_act);
    }

    GameMaster_State_RunSingleTurn GetRunSingleTurnState()
    {
        return m_Runner.GetStateObject<GameMaster_State_RunSingleTurn>((int)GameMasterState.RunSingleTurn);
    }
}
