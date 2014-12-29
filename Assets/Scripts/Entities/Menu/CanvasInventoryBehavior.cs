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
        private Text[] m_InventoryTextField;

        [SerializeField]
        private Image[] m_InventoryImageField;

        void Start()
        {
            if (Debug.isDebugBuild)
            {
                GameStateManager.GetInstance().LoadDefaultWeaponInventory();
                GameStateManager.GetInstance().LoadDefaultItemInventory();
            }

            m_ItemsButton.Send(WidgetEvent.Select);//Select();
        }

        public void ShowItems()
        {
            int textFieldId = 0;
            foreach (Item wep in GameStateManager.GetInstance().ItemsInventory)
            {
                if (textFieldId >= m_InventoryTextField.Length)
                    break;

                Text textWidget = m_InventoryTextField[textFieldId];
                textWidget.text = wep.m_ItemWeapon;

                m_InventoryImageField[textFieldId].sprite = wep.m_Image;

                textWidget.gameObject.SetActive(true);
                m_InventoryImageField[textFieldId].gameObject.SetActive(true);

                ++textFieldId;
            }

            for (int i = textFieldId; i < m_InventoryTextField.Length; ++i)
            {
                m_InventoryTextField[i].gameObject.SetActive(false);
                m_InventoryImageField[i].gameObject.SetActive(false);
            }
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

                m_InventoryImageField[textFieldId].sprite = wep.m_Image;

                textWidget.gameObject.SetActive(true);
                m_InventoryImageField[textFieldId].gameObject.SetActive(true);

                ++textFieldId;
            }

            for(int i = textFieldId; i < m_InventoryTextField.Length; ++i)
            {
                m_InventoryTextField[i].gameObject.SetActive(false);
                m_InventoryImageField[i].gameObject.SetActive(false);
            }
        }
    }
}
