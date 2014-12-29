using Assets.Scripts.Assets;
using Assets.Scripts.Entities.World;

using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class MagicManager
    {
        private Dictionary<MagicId, MagicDescription> m_lookUpTable;

        private const string ASSET_PATH = "Magic";

        private static MagicManager m_Instance = null;

        private MagicManager() 
        {
            m_lookUpTable = new Dictionary<MagicId, MagicDescription>();

            LoadAllMagicDescriptions();
        }

        public static MagicManager GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new MagicManager();

            return m_Instance;
        }

        public MagicDescription GetDescription(MagicId _id)
        {
            if (m_lookUpTable.ContainsKey(_id))
                return m_lookUpTable[_id];

            Debug.LogError(string.Format("Can't find MagicDescription with a MagicId {0}", _id));
            return null;
        }

        private void LoadAllMagicDescriptions()
        {
            MagicDescription[] loadedAssets = Resources.LoadAll<MagicDescription>(ASSET_PATH);
            foreach (MagicDescription desc in loadedAssets)
                m_lookUpTable.Add(desc.m_Id, desc);
        }
    }
}
