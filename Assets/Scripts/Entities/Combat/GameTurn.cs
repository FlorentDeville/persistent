using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class GameTurn
    {
        public List<GameObject> m_Pawns;

        public GameTurn()
        {
            m_Pawns = new List<GameObject>();
        }
    }
}
