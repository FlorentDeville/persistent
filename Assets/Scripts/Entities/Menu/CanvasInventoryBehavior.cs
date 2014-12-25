using Assets.Scripts.Assets;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasInventoryBehavior : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private CustomButton m_ItemsButton;

        [SerializeField]
        private CustomButton m_WeaponsButton;

        [SerializeField]
        private Text[] m_InventoryTextField;

        void Start()
        {
            if(Debug.isDebugBuild)
                GameStateManager.GetInstance().LoadDefaultWeaponInventory();

            m_ItemsButton.Select();
        }

        public void ShowItems()
        {
            foreach (Text widget in m_InventoryTextField)
                widget.gameObject.SetActive(false);
        }

        public void ShowWeapons()
        { 
            int textFieldId = 0;
            foreach(Weapon wep in GameStateManager.GetInstance().WeaponInventory)
            {
                if(textFieldId >= m_InventoryTextField.Length)
                    break;

                Text textWidget = m_InventoryTextField[textFieldId];
                textWidget.text = wep.m_WeaponName;

                textWidget.gameObject.SetActive(true);
                ++textFieldId;
            }

            for(int i = textFieldId; i < m_InventoryTextField.Length; ++i)
            {
                m_InventoryTextField[textFieldId].gameObject.SetActive(false);
            }
        }
    }
}
