using Assets.Scripts.Assets;
using Assets.Scripts.UI;
using Assets.Scripts.Manager;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasEquipementBehavior : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private CustomButton[] m_BtnCharacters;

        [SerializeField]
        private Canvas m_WeaponSelection;

        [SerializeField]
        private Text m_TxtWepAtk;

        [SerializeField]
        private Text m_TxtWepRAtk;

        [SerializeField]
        private Text m_TxtWepMGAtk;

        [SerializeField]
        private Text m_TxtWepMGRAtk;

        [SerializeField]
        private Text m_TxtWeaponName;

        [SerializeField]
        private CanvasWeaponSelectionBehavior m_CanvasWeaponSelector;

        private int m_SelectedCharacterId;

        void Awake()
        {
            m_WeaponSelection.gameObject.SetActive(false);
        }

        void Start()
        {
            InitializeCharactersButtons();
            m_BtnCharacters[0].Select();

            m_CanvasWeaponSelector.m_OnWeaponSelected = OnWeaponSelected;
            m_CanvasWeaponSelector.m_OnCanvasClose = OnCanvasWeaponsClose;
        }

        private void InitializeCharactersButtons()
        {
            if (Debug.isDebugBuild)
            {
                GameStateManager.GetInstance().GetCharacter(0);
                GameStateManager.GetInstance().GetCharacter(1);
            }

            int btnId = 0;
            foreach (Character chara in GameStateManager.GetInstance().Characters)
            {
                if (m_BtnCharacters.Length <= btnId)
                    break;

                m_BtnCharacters[btnId].GetComponentInChildren<Text>().text = chara.m_Name;
                int charaId = chara.m_Id;
                m_BtnCharacters[btnId].onSelect.AddListener(() => { ShowCharacteristics(charaId); });
                m_BtnCharacters[btnId].onClick.AddListener(() => { OnSelectCharacter(charaId); });

                ++btnId;
            }

            for (int i = btnId; i < m_BtnCharacters.Length; ++i)
                m_BtnCharacters[btnId].gameObject.SetActive(false);
        }

        private void ShowCharacteristics(int _charaId)
        {
            m_SelectedCharacterId = _charaId;
            Character chara = GameStateManager.GetInstance().GetCharacter(_charaId);
            if (chara == null || chara.m_EquippedWeapon == null)
            {
                m_TxtWeaponName.text = "N/A";
                m_TxtWepAtk.text = "N/A";
                m_TxtWepRAtk.text = "N/A";
                m_TxtWepMGAtk.text = "N/A";
                m_TxtWepMGRAtk.text = "N/A";
                return;
            }
                

            m_TxtWeaponName.text = chara.m_EquippedWeapon.m_WeaponName;
            m_TxtWepAtk.text = chara.m_EquippedWeapon.m_Atk.ToString();
            m_TxtWepRAtk.text = chara.m_EquippedWeapon.m_AtkR.ToString();
            m_TxtWepMGAtk.text = chara.m_EquippedWeapon.m_MGAtk.ToString();
            m_TxtWepMGRAtk.text = chara.m_EquippedWeapon.m_MGAtkR.ToString();
        }

        private void OnSelectCharacter(int _charaId)
        {
            m_SelectedCharacterId = _charaId;
            LoseFocus();
            Weapon equippedWeapon = GameStateManager.GetInstance().GetCharacter(_charaId).m_EquippedWeapon;
            m_CanvasWeaponSelector.Activate(equippedWeapon);
        }

        private void OnWeaponSelected(Weapon _weapon)
        {
            Character chara = GameStateManager.GetInstance().GetCharacter(m_SelectedCharacterId);
            chara.m_EquippedWeapon = _weapon;
            ShowCharacteristics(m_SelectedCharacterId);
            m_CanvasWeaponSelector.gameObject.SetActive(false);

            OnCanvasWeaponsClose();
        }

        private void OnCanvasWeaponsClose()
        {
            foreach (CustomButton btn in m_BtnCharacters)
            {
                if (btn.gameObject.activeInHierarchy)
                    btn.HandleInput = true;
            }
        }

        private void LoseFocus()
        {
            foreach (CustomButton btn in m_BtnCharacters)
                btn.HandleInput = false;
        }

        private void SetFocus()
        {
            foreach (CustomButton btn in m_BtnCharacters)
                btn.HandleInput = true;
        }
    }
}
