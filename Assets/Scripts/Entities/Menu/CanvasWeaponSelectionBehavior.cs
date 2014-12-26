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
#pragma warning disable 649

        [SerializeField]
        private CustomButton[] m_BtnInventory;

        [SerializeField]
        private Text m_AtkIncrease;

        [SerializeField]
        private Text m_RAtkIncrease;

        [SerializeField]
        private Text m_MGAtkIncrease;

        [SerializeField]
        private Text m_MGRAtkIncrease;

        public delegate void OnWeaponSelectedCallback(Weapon _weapon);
        public OnWeaponSelectedCallback m_OnWeaponSelected;

        public delegate void OnCanvasCloseCallback();
        public OnCanvasCloseCallback m_OnCanvasClose;

        private Weapon m_EquipedWeapon;

        void InitializeWeaponList()
        {
            if (Debug.isDebugBuild)
                GameStateManager.GetInstance().LoadDefaultWeaponInventory();

            int textFieldId = 0;
            foreach (Weapon wep in GameStateManager.GetInstance().WeaponInventory)
            {
                if (textFieldId >= m_BtnInventory.Length)
                    break;

                if (GameStateManager.GetInstance().IsEquipped(wep))
                    continue;

                Text textWidget = m_BtnInventory[textFieldId].GetComponentInChildren<Text>();
                textWidget.text = wep.m_WeaponName;


                Image[] allImages = m_BtnInventory[textFieldId].GetComponentsInChildren<Image>();
                Image imgWidget = allImages[allImages.Length - 1];
                imgWidget.sprite = wep.m_Image;

                Weapon w = wep;
                m_BtnInventory[textFieldId].onSelect.AddListener(() => { Compare(m_EquipedWeapon, w); });
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

        public void Activate(Weapon _equipedWeapon)
        {
            m_EquipedWeapon = _equipedWeapon;
            gameObject.SetActive(true);
            InitializeWeaponList();
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

        private void Compare(Weapon _equiped, Weapon _selected)
        {
            if (_equiped == null)
            {
                SetComparisonValue(m_AtkIncrease, _selected.m_Atk);
                SetComparisonValue(m_RAtkIncrease, _selected.m_AtkR);
                SetComparisonValue(m_MGAtkIncrease, _selected.m_MGAtk);
                SetComparisonValue(m_MGRAtkIncrease, _selected.m_MGAtkR);
                return;
            }

            SetComparison(m_AtkIncrease, _equiped.m_Atk, _selected.m_Atk);
            SetComparison(m_RAtkIncrease, _equiped.m_AtkR, _selected.m_AtkR);
            SetComparison(m_MGAtkIncrease, _equiped.m_MGAtk, _selected.m_MGAtk);
            SetComparison(m_MGRAtkIncrease, _equiped.m_MGAtkR, _selected.m_MGAtkR);
        }

        private void SetComparison(Text _widget, float _currentValue, float _newValue)
        {
            float diff = _newValue - _currentValue;
            SetComparisonValue(_widget, diff);
        }

        private void SetComparisonValue(Text _widget, float _value)
        {
            string strValue;
            Color textColor;
            if (_value > 0)
            {
                strValue = string.Format("+{0}", _value);
                textColor = Color.green;
            }
            else if(_value < 0)
            {
                strValue = string.Format("{0}", _value);
                textColor = Color.red;
            }
            else
            {
                strValue = string.Format("{0}", _value);
                textColor = Color.blue;
            }

            _widget.text = strValue;
            _widget.color = textColor;
            //m_AtkIncrease.text = "+" + _selected.m_Atk.ToString();
            //m_RAtkIncrease.text = "+" + _selected.m_AtkR.ToString();
            //m_MGAtkIncrease.text = "+" + _selected.m_MGAtk.ToString();
            //m_MGRAtkIncrease.text = "+" + _selected.m_MGAtkR.ToString();

            //m_AtkIncrease.color = Color.green;
            //m_RAtkIncrease.color = Color.green;
            //m_MGAtkIncrease.color = Color.green;
            //m_MGRAtkIncrease.color = Color.green;
        }
    }
}
