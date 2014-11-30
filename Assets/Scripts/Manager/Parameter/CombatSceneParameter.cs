using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager.Parameter
{
    public class CombatSceneParameter
    {
        public List<Transform> m_PlayerPawns;

        public List<Transform> m_EnemiesPawns;

        public CombatSceneParameter()
        {
            m_PlayerPawns = new List<Transform>();
            m_EnemiesPawns = new List<Transform>();
        }
    }
}
