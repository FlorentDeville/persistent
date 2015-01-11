using Assets.Scripts.Entities.Combat;

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameIteration
    {
        private List<GameObject> m_Pawns;

        public GameIteration()
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

        public void AddTurn(GameObject _pawn)
        {
            m_Pawns.Add(_pawn);
        }

        public int GetCount()
        {
            return m_Pawns.Count;
        }

        public GameObject GetPawn(int _turn)
        {
            return m_Pawns[_turn];
        }
    }

    public class GameTurnManager
    {
        private static GameTurnManager m_Instance = null;

        public GameIteration m_CurrentIteration;

        public List<GameIteration> m_NextIterations;
        
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
            m_NextIterations = new List<GameIteration>();
            m_OrderedPawns = new List<GameObject>();
        }

        public void Init(List<GameObject> _pawns)
        {
            m_NextIterations.Clear();
            m_NextIterations.Capacity = 3;

            m_PlayerPawns = _pawns.FindAll(item => item.GetComponent<PawnStatistics>().m_IsControlledByPlayer);
            m_EnemiesPawns = _pawns.FindAll(item => !item.GetComponent<PawnStatistics>().m_IsControlledByPlayer);

            ComputeOrderedPawn();

            Update();
        }

        public void NextTurn()
        {
            PawnStatistics stats = GetCurrentPawnStatistics();
            stats.m_Priority -= 1;

            Update();
        }

        public void Update()
        {
            CleanUpDeadPawns();

            if (m_OrderedPawns.TrueForAll(item => item.GetComponent<PawnStatistics>().m_Priority < 1))
                ApplyPriorityRestore();

            List<float> priorityWorkingCopy = new List<float>();
            foreach(GameObject obj in m_OrderedPawns)
                priorityWorkingCopy.Add(obj.GetComponent<PawnStatistics>().m_Priority);

            m_CurrentIteration = ComputeIteration(0, priorityWorkingCopy);

            m_NextIterations.Clear();
            int turnCount = m_CurrentIteration.GetCount();
            const int MAX_TURN_COUNT = 10;
            while (turnCount < MAX_TURN_COUNT)
            {
                for (int i = 0; i < m_OrderedPawns.Count; ++i)
                    priorityWorkingCopy[i] += m_OrderedPawns[i].GetComponent<PawnStatistics>().m_PriorityIncrease;
                
                GameIteration iteration = ComputeIteration(0, priorityWorkingCopy);
                m_NextIterations.Add(iteration);

                turnCount += iteration.GetCount();
            }
        }

        public GameObject GetCurrentPawn()
        {
            return m_CurrentIteration.GetPawn(0);
        }

        public PawnStatistics GetCurrentPawnStatistics()
        {
            GameObject obj = GetCurrentPawn();

            return obj.GetComponent<PawnStatistics>();
        }

        public List<GameObject> Preview(GameObject _previewPawn, PawnStatistics _previewStatistics, int _count)
        {
            Dictionary<GameObject, PawnStatistics> statistics = new Dictionary<GameObject, PawnStatistics>();
            foreach(GameObject obj in m_OrderedPawns)
            {
                if (obj == _previewPawn)
                    statistics.Add(obj, _previewStatistics);
                else
                    statistics.Add(obj, obj.GetComponent<PawnStatistics>());
            }

            List<GameObject> ret = new List<GameObject>();

            List<float> priorityBase = new List<float>();
            foreach (GameObject obj in m_OrderedPawns)
                priorityBase.Add(statistics[obj].m_Priority);

            int turnCount = 0;
            while(turnCount < _count)
            {
                GameIteration iter = ComputeIteration(0, priorityBase);
                for (int i = 0; i < iter.GetCount(); ++i)
                    ret.Add(iter.GetPawn(i));

                turnCount += iter.GetCount();

                for (int i = 0; i < m_OrderedPawns.Count; ++i )
                    priorityBase[i] += statistics[m_OrderedPawns[i]].m_PriorityIncrease;
            }

            return ret;
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

            m_OrderedPawns.RemoveAll(item =>
            {
                return item.GetComponent<PawnBehavior>().m_State == PawnState.Dead;
            });
        }

        private void ApplyPriorityRestore()
        {
            foreach(GameObject obj in m_OrderedPawns)
            {
                PawnStatistics stats = obj.GetComponent<PawnStatistics>();
                stats.m_Priority += stats.m_PriorityIncrease;
            }
        }

        private GameIteration ComputeIteration(int _startPawnId, List<float> _priorityBase)
        {
            const int PRIORITY_THRESHOLD = 1;

            GameIteration iteration = new GameIteration();
            bool atLeastOneTurnAdded = true;
            while(atLeastOneTurnAdded)
            {
                atLeastOneTurnAdded = false;
                for (int i = _startPawnId; i < m_OrderedPawns.Count; ++i)
                {
                    if(_priorityBase[i] >= PRIORITY_THRESHOLD)
                    {
                        iteration.AddTurn(m_OrderedPawns[i]);
                        _priorityBase[i] -= PRIORITY_THRESHOLD;
                        atLeastOneTurnAdded = true;
                    }
                }
            }

            return iteration;
        }
    }
}
