﻿using Assets.Scripts.Entities.Combat;

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameTurn
    {
        public List<GameObject> m_Pawns;

        public GameTurn()
        {
            m_Pawns = new List<GameObject>();
        }

        public void CleanDeadPawn()
        {
            m_Pawns.RemoveAll(pawn =>
                {
                    return pawn.GetComponent<PawnBehavior>().m_State == PawnState.Dead;
                });
        }
    }

    public class GameTurnManager
    {
        private static GameTurnManager m_Instance = null;

        public GameTurn m_currentTurn;
        public int m_PlayingPawnIdInCurrentTurn;

        public List<GameTurn> m_TurnPredictions;
        public int m_TurnPredictionCount;
        
        private List<GameObject> m_Pawns;
        public List<GameObject> m_PlayerPawns;
        public List<GameObject> m_EnemiesPawns;
        private List<GameObject> m_OrderedPawns;

        public static GameTurnManager GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GameTurnManager();

            return m_Instance;
        }

        private GameTurnManager()
        {
            m_TurnPredictions = new List<GameTurn>();
            m_OrderedPawns = new List<GameObject>();
        }

        public void Init(List<GameObject> _pawns)
        {
            m_TurnPredictionCount = 2;
            m_TurnPredictions.Clear();
            m_TurnPredictions.Capacity = 2;

            m_Pawns = _pawns;
            m_PlayerPawns = _pawns.FindAll(item => item.GetComponent<PawnStatistics>().m_IsControlledByPlayer);
            m_EnemiesPawns = _pawns.FindAll(item => !item.GetComponent<PawnStatistics>().m_IsControlledByPlayer);

            ComputeOrderedPawn();

            m_currentTurn = ComputeTurn();
            m_PlayingPawnIdInCurrentTurn = 0;

            ComputePredictionTurns();
        }

        public void NextTurn()
        {
            ++m_PlayingPawnIdInCurrentTurn;
            Update();
        }

        public void Update()
        {
            if (m_PlayingPawnIdInCurrentTurn >= m_currentTurn.m_Pawns.Count)
            {
                m_currentTurn = ComputeTurn();
                m_PlayingPawnIdInCurrentTurn = 0;                
            }
            else
            {
                m_currentTurn.CleanDeadPawn();
            }

            CleanUpDeadPawns();
            ComputeOrderedPawn();
            ComputePredictionTurns();
        }

        public GameObject GetCurrentPawn()
        {
            return m_currentTurn.m_Pawns[m_PlayingPawnIdInCurrentTurn];
        }

        public PawnStatistics GetCurrentPawnStatistics()
        {
            GameObject obj = GetCurrentPawn();

            return obj.GetComponent<PawnStatistics>();
        }

        public bool IsLastPawnInCurrentTurn()
        {
            return m_currentTurn.m_Pawns.Count - 1 == m_PlayingPawnIdInCurrentTurn;
        }

        private void ComputePredictionTurns()
        {
            for(int turnPredictionId = 0; turnPredictionId < m_TurnPredictionCount; ++turnPredictionId)
            {
                GameTurn turn = ComputeTurn();

                if(m_TurnPredictions.Count <= turnPredictionId)
                    m_TurnPredictions.Add(turn);
                else
                    m_TurnPredictions[turnPredictionId] = turn; 
            }
        }

        private GameTurn ComputeTurn()
        {
            const int PRIORITY_THRESHOLD = 1;

            GameTurn turn = new GameTurn();
            for (int pawnId = 0; pawnId < m_OrderedPawns.Count; ++pawnId)
            {
                PawnBehavior bhv = m_OrderedPawns[pawnId].GetComponent<PawnBehavior>();
                if (bhv.m_State == PawnState.Dead)
                    continue;

                PawnStatistics stat = m_OrderedPawns[pawnId].GetComponent<PawnStatistics>();
                if (stat.m_Priority >= PRIORITY_THRESHOLD) // if the priority is above threshold, add turn
                {
                    turn.m_Pawns.Add(m_OrderedPawns[pawnId]);
                    stat.m_Priority -= PRIORITY_THRESHOLD;
                }

                //increase priority.
                stat.m_Priority += stat.m_PriorityIncrease;
            }

            return turn;
        }

        private void ComputeOrderedPawn()
        {
            int maxSize = m_PlayerPawns.Count > m_EnemiesPawns.Count ? m_PlayerPawns.Count : m_EnemiesPawns.Count;
            m_OrderedPawns = new List<GameObject>();

            for (int i = 0; i < maxSize; ++i)
            {
                if (i < m_PlayerPawns.Count)
                    m_OrderedPawns.Add(m_PlayerPawns[i]);

                if (i < m_EnemiesPawns.Count)
                    m_OrderedPawns.Add(m_EnemiesPawns[i]);
            }
        }

        private void CleanUpDeadPawns()
        {
            m_PlayerPawns.RemoveAll(item =>
                {
                    return item.GetComponent<PawnBehavior>().m_State == PawnState.Dead;
                });
            m_EnemiesPawns.RemoveAll(item =>
            {
                return item.GetComponent<PawnBehavior>().m_State == PawnState.Dead;
            });
        }
    }
}