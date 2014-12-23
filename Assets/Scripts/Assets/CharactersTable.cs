using UnityEngine;

namespace Assets.Scripts.Assets
{
    [System.Serializable]
    public class CharacterId
    {
        public string m_PrivateName;
        public int m_Id;
    }

    [System.Serializable]
    public class CharactersTable : ScriptableObject
    {
        public CharacterId[] m_Table;
    }
}
