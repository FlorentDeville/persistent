using Assets.Scripts.Assets;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameStateManager
    {
        private static GameStateManager m_Instance = null;

        private Dictionary<int, Character> m_Characters;

        public Dictionary<int, Character>.ValueCollection Characters
        {
            get { return m_Characters.Values; }
        }

        private List<Weapon> m_WeaponInventory;
        public List<Weapon> WeaponInventory
        {
            get { return m_WeaponInventory; }
        }

        private List<Item> m_ItemsInventory;
        public List<Item> ItemsInventory
        {
            get { return m_ItemsInventory; }
        }

        private GameStateManager()
        {
            m_Characters = new Dictionary<int, Character>();
            m_WeaponInventory = new List<Weapon>();
            m_ItemsInventory = new List<Item>();
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

        public void AddToInventory(Weapon _weapon)
        {
            m_WeaponInventory.Add(_weapon);
        }

        public bool IsEquipped(Weapon _weapon)
        {
            if(_weapon == null)
                return false;

            foreach(Character chara in Characters)
            {
                if (_weapon == chara.m_EquippedWeapon)
                    return true;
            }

            return false;
        }

        public void LoadDefaultWeaponInventory()
        {
            return;
            if (m_WeaponInventory.Count != 0)
                return;

            string relPath = "Weapon";

            string[] weaponsToLoad = new string[]
            {
                "axe"
                , "sword"
                //, "brokensword"
            };

            foreach(string weaponName in weaponsToLoad)
            {
                string path = string.Format("{0}/{1}", relPath, weaponName);
                Weapon weapon = Resources.Load<Weapon>(path);
                m_WeaponInventory.Add(weapon);
            }
            
        }

        public void LoadDefaultItemInventory()
        {
            if (m_ItemsInventory.Count != 0)
                return;

            string relPath = "Items";

            string[] itemsToLoad = new string[]
            {
                "coin",
                "potion",
                "map",
                "wing"
            };

            foreach (string itemName in itemsToLoad)
            {
                string path = string.Format("{0}/{1}", relPath, itemName);
                Item newItem = Resources.Load<Item>(path);
                m_ItemsInventory.Add(newItem);
            }

        }

        private void LoadDefaultCharacter(int _id)
        {
            Character newChar = new Character();
            newChar.m_Id = _id;
            if (_id == 0)
                newChar.m_Name = "Player";
            else
                newChar.m_Name = "Sidekick";

            newChar.m_EquippedWeapon = null;
            newChar.m_Statistics = new CharacterStatistics();

            if(_id == 0)
                newChar.m_Statistics.m_Atk = 300;
            else
                newChar.m_Statistics.m_Atk = 200;
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
