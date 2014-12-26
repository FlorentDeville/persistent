using Assets.Scripts.Assets;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using System.Collections.Generic;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasWeaponSelectionBehavior : MonoBehaviour
    {
        [SerializeField]
        private CustomButton[] m_BtnInventory;

        public delegate void OnWeaponSelectedCallback(Weapon _weapon);
        public OnWeaponSelectedCallback m_OnWeaponSelected;

        public delegate void OnCanvasCloseCallback();
        public OnCanvasCloseCallback m_OnCanvasClose;

        void Start()
        {
            InitializeWeaponList();
            m_BtnInventory[0].Select();
        }

        void InitializeWeaponList()
        {
            if (Debug.isDebugBuild)
                GameStateManager.GetInstance().LoadDefaultWeaponInventory();

            int textFieldId = 0;
            foreach (Weapon wep in GameStateManager.GetInstance().WeaponInventory)
            {
                if (textFieldId >= m_BtnInventory.Length)
                    break;

                Text textWidget = m_BtnInventory[textFieldId].GetComponentInChildren<Text>();
                textWidget.text = wep.m_WeaponName;


                Image[] allImages = m_BtnInventory[textFieldId].GetComponentsInChildren<Image>();
                Image imgWidget = allImages[allImages.Length - 1];
                imgWidget.sprite = wep.m_Image;

                Weapon w = wep;
                m_BtnInventory[textFieldId].onClick.AddListener(() => { onWeaponClicked(ref w); });
                m_BtnInventory[textFieldId].onCancel.AddListener(() => { onCanvasClose(); });

                m_BtnInventory[textFieldId].gameObject.SetActive(true);

                ++textFieldId;
            }

            for (int i = textFieldId; i < m_BtnInventory.Length; ++i)
            {
                m_BtnInventory[i].gameObject.SetActive(false);
            }

        }

        public void Activate()
        {
            gameObject.SetActive(true);
            DeselectAndHandleInputAll();
            m_BtnInventory[0].Select();
        }

        private void onWeaponClicked(ref Weapon wep)
        {
            m_OnWeaponSelected(wep);
        }

        private void onCanvasClose()
        {
            gameObject.SetActive(false);
            m_OnCanvasClose();
        }

        private void DeselectAndHandleInputAll()
        {
            foreach(CustomButton btn in m_BtnInventory)
            {
                if(btn.gameObject.activeInHierarchy)
                {
                    btn.Deselect();
                    btn.HandleInput = true;
                }
            }
        }
    }
}
