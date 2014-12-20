using Assets.Scripts.Entities.Combat;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_TurnHistory : MonoBehaviour
{
    public GameObject[] m_HistoryTurnImages;

    public void UpdateTurnHistory(GameTurnManager _manager)
    {
        int historyCount = m_HistoryTurnImages.Length;

        //Set the current turn
        GameTurn currentTurn = _manager.m_currentTurn;
        int currentTurnId = 0;
        foreach(GameObject pawnGameObject in currentTurn.m_Pawns)
        {
            if (currentTurnId >= historyCount)
                return;

            SetHistorySprite(currentTurnId, pawnGameObject);
            ++currentTurnId;
        }

        //Set the prediction turn
        foreach(GameTurn turnPrediction in _manager.m_TurnPredictions)
        {
            foreach (GameObject pawnGameObject in turnPrediction.m_Pawns)
            {
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

    private void SetHistorySprite(int _turnId, GameObject _pawn)
    {
        PawnUI pawnUIComponent = _pawn.GetComponent<PawnUI>();
        if (pawnUIComponent == null)
        {
            Debug.LogError("The pawn " + _pawn.name + " has no PawnUI component.");
            return;
        }

        Image img = m_HistoryTurnImages[_turnId].GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("The game object " + m_HistoryTurnImages[_turnId].name + " has no Image component.");
            return;
        }

        img.sprite = pawnUIComponent.m_TurnSprite;
    }

    private void SetHistorySpriteToNull(int _turnId)
    {
        Image img = m_HistoryTurnImages[_turnId].GetComponent<Image>();
        if (img == null)
        {
            Debug.LogError("The game object " + m_HistoryTurnImages[_turnId].name + " has no Image component.");
            return;
        }

        img.sprite = null;
    }
}

