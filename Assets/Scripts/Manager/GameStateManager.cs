using Assets.Scripts.Assets;
using System.Collections.Generic;

namespace Assets.Scripts.Manager
{
    public class GameStateManager
    {
        private static GameStateManager m_Instance = null;

        public List<Character> m_Characters;

        private GameStateManager()
        {
            m_Characters = new List<Character>();
        }

        public static GameStateManager GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GameStateManager();

            return m_Instance;
        }
    }

    public class Character
    {
        public string m_Name;

        public CharacterStatistics m_Statistics;

        public Weapon m_EquippedWeapon;
    }

    public class CharacterStatistics
    {
        public float m_Atk;

        public float m_Def;

        public float m_MaxHP;

        public float m_HP;

        public float m_MaxMP;

        public float m_MP;

        public float m_MGAtk;

        public float m_MGDef;

        public float m_Priority;

        public float m_PriorityIncrease;
    }
}
