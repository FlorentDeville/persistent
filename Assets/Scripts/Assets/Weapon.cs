using UnityEngine;

namespace Assets.Scripts.Assets
{
    [System.Serializable]
    public class Weapon : ScriptableObject
    {
        public string Name;

        public float m_Atk;

        public float m_AtkR;

        public float m_MGAtk;

        public float m_MGAtkR;
    }
}
