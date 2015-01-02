using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

using Assets.Scripts.Component.Actions;
using Assets.Scripts.Manager.Parameter;
using Assets.Scripts.Manager;
using Assets.Scripts.Entities.Combat;

using AssemblyCSharp;
using Assets.Scripts.Entities.UI;
using Assets.Scripts.UI;

#pragma warning disable 649

[RequireComponent(typeof(Combat_Settings))]
public partial class GameMaster : MonoBehaviour 
{
    public UnityEngine.EventSystems.EventSystem m_EventSystem;

    public CombatUI_TurnHistory m_UITurnHistory;

    public CombatUI_PawnState[] m_UIPawnState;

    [SerializeField]
    private CombatUI_CanvasActions m_ActionsMenu;

    public CancelButton[] m_UIAttackEnemyButtons;

    public GameObject m_Cursor;

    public GameObject m_UIDamageText;
    public GameObject m_UIGameOverText;
    public GameObject m_UIVictoryText;

    public CombatUI_Effect m_UIEffects;

    #region Variable State Init

    [Header("State : Init")]
    public RawImage m_InitFadeWidget;

    public Color m_InitColor;

    #endregion

    #region Variable State PlayIntro

    [Header("State : Play Intro")]
    public CinemaDirector.Cutscene m_IntroCutscene;

    public float m_ColorScreenDuration;

    #endregion

    public enum GameMasterState
    {
        Init,
        PlayIntro,
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

    private static GameMaster m_Instance = null;

    public static GameMaster GetInstance()
    {
        if (m_Instance == null)
            m_Instance = FindObjectOfType<GameMaster>();

        return m_Instance;
    }

	// Use this for initialization
	void Start () 
    {
        m_Runner = new FSMRunner(gameObject);
        
        m_Runner.RegisterState<GameMaster_State_RunSingleTurn>();
        m_Runner.RegisterState<GameMaster_State_CheckGoal>();
        m_Runner.RegisterState<GameMaster_State_GameOver>();
        m_Runner.RegisterState<GameMaster_State_Victory>();
        m_Runner.RegisterState<GameMaster_State_Terminate>();
        m_Runner.RegisterState<GameMaster_State_DummyLoop>();

        GameMaster_State_Init stateInit = m_Runner.RegisterState<GameMaster_State_Init>();
        stateInit.m_FadeWidget = m_InitFadeWidget;
        stateInit.m_InitColor = m_InitColor;

        GameMaster_State_PlayIntro statePlayIntro = m_Runner.RegisterState<GameMaster_State_PlayIntro>();
        statePlayIntro.m_IntroCutscene = m_IntroCutscene;

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

            StickToGround.Apply(pawn.gameObject);
        }

        for(int prefabId = 0; prefabId < m_SceneParameter.m_PlayerPawns.Count; prefabId++)
        {
            Transform prefab = m_SceneParameter.m_PlayerPawns[prefabId];

            Vector3 position = m_Settings.ComputePawnPlayerGlobalPosition(prefabId);
            Quaternion orientation = m_Settings.ComputePawnPlayerGlobalOrientation(prefabId);
            Transform pawn = (Transform)Instantiate(prefab, position, orientation);
            pawn.parent = gameObject.transform.root;
            m_Pawns.Add(pawn.gameObject);

            StickToGround.Apply(pawn.gameObject);
        }

        m_UIDamageText.SetActive(false);
        m_UIGameOverText.SetActive(false);
        m_UIVictoryText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_Runner.Update();
	}

    void LateUpdate()
    {
        m_Runner.LateUpdate();
    }

    void OnAttackClicked()
    {
        //GameTurnManager turnMng = GameTurnManager.GetInstance();
        //int enemyCount = turnMng.m_EnemiesPawns.Count;
        //int buttonUsed = 0;
        //for(int i = 0; i < enemyCount; ++i)
        //{
        //    GameObject objPawn = turnMng.m_EnemiesPawns[i];
        //    PawnBehavior bhv = objPawn.GetComponent<PawnBehavior>();
        //    if (bhv.m_State == PawnState.Dead)
        //        continue;

        //    CancelButton btn = m_UIAttackEnemyButtons[buttonUsed];
        //    btn.gameObject.SetActive(true);
        //    Text txt = btn.transform.GetComponentInChildren<Text>();
        //    txt.text = objPawn.GetComponent<PawnStatistics>().m_PawnName;
        //    btn.onClick.AddListener(() =>
        //        {
        //            HideAllAttackEnemiesButtons();

        //            IAction attackAction = turnMng.GetCurrentPawn().GetComponent<PawnActions>().GetAttackAction();
        //            attackAction.SetTarget(objPawn);
        //            SetSelectedAction(attackAction);

        //        });

        //    btn.onCancel.AddListener(() =>
        //        {
        //            HideAllAttackEnemiesButtons();
        //            m_AttackButton.Select();
        //            m_Cursor.SetActive(false);
        //            m_MagicButton.gameObject.SetActive(true);
        //            m_ItemsButton.gameObject.SetActive(true);
        //        });

        //    btn.onSelect.AddListener(() =>
        //        {
        //            Vector3 camPosition = Camera.main.WorldToScreenPoint(objPawn.transform.position);
        //            m_Cursor.transform.position = camPosition;
        //        });

        //    ++buttonUsed;
        //}

        //for (int i = buttonUsed; i < m_UIAttackEnemyButtons.Length; ++i)
        //{
        //    Button btn = m_UIAttackEnemyButtons[i];
        //    btn.gameObject.SetActive(false);
        //}

        //m_UIAttackEnemyButtons[0].Select();
        //m_Cursor.SetActive(true);
        //m_MagicButton.gameObject.SetActive(false);
        //m_ItemsButton.gameObject.SetActive(false);
    }

    public void SetSelectedAction(ActionRunner _act)
    {
        GameMaster_State_RunSingleTurn runSingleTurnState = GetRunSingleTurnState();
        runSingleTurnState.SetSelectedAction(_act);
    }

    public void ActionReady()
    {
        GameMaster_State_RunSingleTurn runSingleTurnState = GetRunSingleTurnState();
        runSingleTurnState.ActionReady();
    }

    public ActionRunner GetSelectedAction()
    {
        GameMaster_State_RunSingleTurn runSingleTurnState = GetRunSingleTurnState();
        return runSingleTurnState.GetSelectedAction();
    }

    GameMaster_State_RunSingleTurn GetRunSingleTurnState()
    {
        return m_Runner.GetStateObject<GameMaster_State_RunSingleTurn>((int)GameMasterState.RunSingleTurn);
    }

    public void ShowActionsMenu()
    {
        WidgetManager.GetInstance().Show(m_ActionsMenu.gameObject, false, false);
        m_ActionsMenu.Show();
    }

    public void HideActionsMenu()
    {
        WidgetManager.GetInstance().Hide();
        m_ActionsMenu.Hide();
    }
}
