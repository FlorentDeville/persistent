using Assets.Scripts.Entities.Combat;
using Assets.Scripts.Manager;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

public class CombatUI_TurnHistory : MonoBehaviour
{
    [SerializeField]
    [Header("History")]
    private GameObject[] m_HistoryTurnImages;

    [SerializeField]
    [Header("Highlighted Selection")]
    private string m_TriggerHighlight;

    [SerializeField]
    private string m_TriggerNormal;

    [SerializeField]
    [Header("Preview")]
    private GameObject m_PreviewWrapper;

    [SerializeField]
    private GameObject[] m_PreviewHistory;

    private List<GameObject> m_HighlightedTurnItem;

    void Awake()
    {
        m_HighlightedTurnItem = new List<GameObject>();
        HidePreview();
    }

    public void UpdateTurnHistory(GameTurnManager _manager)
    {
        int historyCount = m_HistoryTurnImages.Length;

        //Set the current turn
        GameIteration currentTurn = _manager.m_CurrentIteration;
        int currentTurnId = 0;
        for (int i = 0; i < currentTurn.GetCount(); ++i)
        {
            GameObject pawnGameObject = currentTurn.GetPawn(i);
            if (currentTurnId >= historyCount)
                return;

            SetHistorySprite(currentTurnId, pawnGameObject);
            ++currentTurnId;
        }

        //Set the prediction turn
        foreach(GameIteration turnPrediction in _manager.m_NextIterations)
        {
            for (int i = 0; i < turnPrediction.GetCount(); ++i)
            {
                GameObject pawnGameObject = turnPrediction.GetPawn(i);
                if (currentTurnId >= historyCount)
                    return;

                SetHistorySprite(currentTurnId, pawnGameObject);
                ++currentTurnId;
            }
        }

        //Leave blank space at the end.
        while(currentTurnId < historyCount)
        {
            SetHistorySpriteToNull(currentTurnId);
            ++currentTurnId;
        }
        
    }

    public void HighlightEnemy(GameObject _pawnToHighlight)
    {
        GameIteration currentTurn = GameTurnManager.GetInstance().m_CurrentIteration;
        HighlightEnemyFromGameTurn(_pawnToHighlight, currentTurn, 0);

        int offset = currentTurn.GetCount();// -GameTurnManager.GetInstance().m_CurrentTurnIdInCurrentIteration;
        foreach (GameIteration turn in GameTurnManager.GetInstance().m_NextIterations)
        {
            HighlightEnemyFromGameTurn(_pawnToHighlight, turn, offset);
            offset += turn.GetCount();
        }
    }

    public void RemoveHighlightedEnemies()
    {
        foreach(GameObject obj in m_HighlightedTurnItem)
        {
            Animator anim = obj.GetComponentInChildren<Animator>();
            anim.SetTrigger(m_TriggerNormal);
        }
        m_HighlightedTurnItem.Clear();
    }

    public void ShowPreview(GameObject[] _pawnsList)
    {
        m_PreviewWrapper.SetActive(true);

        for (int imgId = 0, pawnId = 1; pawnId < _pawnsList.Length && imgId < m_PreviewHistory.Length; ++imgId, ++pawnId)
        {
            PawnUI ui = _pawnsList[pawnId].GetComponent<PawnUI>();
            Image img = m_PreviewHistory[imgId].GetComponentInChildren<Image>();
            img.sprite = ui.m_TurnSprite;
        }
    }

    public void HidePreview()
    {
        m_PreviewWrapper.SetActive(false);
    }

    private void SetHistorySprite(int _turnId, GameObject _pawn)
    {
        PawnUI pawnUIComponent = _pawn.GetComponent<PawnUI>();
        if (pawnUIComponent == null)
        {
            Debug.LogError("The pawn " + _pawn.name + " has no PawnUI component.");
            return;
        }

        Image img = m_HistoryTurnImages[_turnId].GetComponentInChildren<Image>();
        if (img == null)
        {
            Debug.LogError("The game object " + m_HistoryTurnImages[_turnId].name + " has no Image component.");
            return;
        }

        img.sprite = pawnUIComponent.m_TurnSprite;
    }

    private void SetHistorySpriteToNull(int _turnId)
    {
        Image img = m_HistoryTurnImages[_turnId].GetComponentInChildren<Image>();
        if (img == null)
        {
            Debug.LogError("The game object " + m_HistoryTurnImages[_turnId].name + " has no Image component.");
            return;
        }

        img.sprite = null;
    }

    private void HighlightEnemyFromGameTurn(GameObject _pawnToHighlight, GameIteration _turn, int _offset)
    {
        for (int i = 0; i < _turn.GetCount(); ++i)
        {
            GameObject obj = _turn.GetPawn(i);
            if (obj == _pawnToHighlight)
            {
                int id = i + _offset;
                if (id < m_HistoryTurnImages.Length && id > 0)
                {
                    Animator anim = m_HistoryTurnImages[id].GetComponentInChildren<Animator>();
                    anim.SetTrigger(m_TriggerHighlight);
                    m_HighlightedTurnItem.Add(m_HistoryTurnImages[id]);
                }
            }
        }
    }
}

