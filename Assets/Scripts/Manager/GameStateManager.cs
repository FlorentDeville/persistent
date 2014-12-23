using Assets.Scripts.Assets;
using System.Collections.Generic;

namespace Assets.Scripts.Manager
{
    public class GameStateManager
    {
        private static GameStateManager m_Instance = null;

        private Dictionary<int, Character> m_Characters;

        private GameStateManager()
        {
            m_Characters = new Dictionary<int, Character>();
        }

        public static GameStateManager GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GameStateManager();

            return m_Instance;
        }

        public Character GetCharacter(int _id)
        {
            if (!m_Characters.ContainsKey(_id))
                LoadDefaultCharacter(_id);

            return m_Characters[_id];
        }

        private void LoadDefaultCharacter(int _id)
        {
            Character newChar = new Character();
            newChar.m_Id = _id;
            newChar.m_Name = "Default";
            newChar.m_EquippedWeapon = null;
            newChar.m_Statistics = new CharacterStatistics();

            newChar.m_Statistics.m_Atk = 300;
            newChar.m_Statistics.m_Def = 2;
            newChar.m_Statistics.m_HP = 100;
            newChar.m_Statistics.m_MaxHP = 100;
            newChar.m_Statistics.m_MP = 20;
            newChar.m_Statistics.m_MaxMP = 20;
            newChar.m_Statistics.m_MGAtk = 10;
            newChar.m_Statistics.m_MGDef = 5;
            newChar.m_Statistics.m_Priority = 1;
            newChar.m_Statistics.m_PriorityIncrease = 1;

            m_Characters.Add(_id, newChar);
        }
    }

    public class Character
    {
        public int m_Id;

        public string m_Name;

        public CharacterStatistics m_Statistics;

        public Weapon m_EquippedWeapon;

        public void LoadTo(PawnStatistics _stats)
        {
            _stats.m_Atk = m_Statistics.m_Atk;
            _stats.m_Def = m_Statistics.m_Def;
            _stats.m_HP = m_Statistics.m_HP;
            _stats.m_MaxHP = m_Statistics.m_MaxHP;
            _stats.m_MP = m_Statistics.m_MP;
            _stats.m_MaxMP = m_Statistics.m_MaxMP;
            _stats.m_MGAtk = m_Statistics.m_MGAtk;
            _stats.m_MGDef= m_Statistics.m_MGDef;
            _stats.m_Priority = m_Statistics.m_Priority;
            _stats.m_PriorityIncrease = m_Statistics.m_PriorityIncrease;
        }

        public void SaveFrom(PawnStatistics _stats)
        {
            m_Statistics.m_Atk = _stats.m_Atk;
            m_Statistics.m_Def = _stats.m_Def;
            m_Statistics.m_HP = _stats.m_HP;
            m_Statistics.m_MaxHP = _stats.m_MaxHP;
            m_Statistics.m_MP = _stats.m_MP;
            m_Statistics.m_MaxMP = _stats.m_MaxMP;
            m_Statistics.m_MGAtk = _stats.m_MGAtk;
            m_Statistics.m_MGDef = _stats.m_MGDef;
            m_Statistics.m_Priority = _stats.m_Priority;
            m_Statistics.m_PriorityIncrease = _stats.m_PriorityIncrease;
        }
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
