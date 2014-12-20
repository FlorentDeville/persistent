using UnityEngine;

namespace Assets.Scripts.Entities.Combat
{
    public class PawnMagicInventory : MonoBehaviour
    {
        public MagicSpell[] m_MagicSpells;
    }

    [System.Serializable]
    public class MagicSpell
    {
        public string m_Name;
        public bool m_Enabled;
    }
}
