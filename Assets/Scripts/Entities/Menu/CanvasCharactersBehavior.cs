using Assets.Scripts.Helper;
using Assets.Scripts.Manager;
using Assets.Scripts.UI;

using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasCharactersBehavior : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private CustomButton[] m_BtnCharacters;

        [SerializeField]
        private Text m_TxtAtk;

        [SerializeField]
        private Text m_TxtDef;

        [SerializeField]
        private Text m_TxtMGAtk;

        [SerializeField]
        private Text m_TxtMGDef;

        [SerializeField]
        private Text m_TxtHP;

        [SerializeField]
        private Text m_TxtMP;

        [SerializeField]
        private Text m_TxtWepAtk;

        [SerializeField]
        private Text m_TxtWepRAtk;

        [SerializeField]
        private Text m_TxtWepMGAtk;

        [SerializeField]
        private Text m_TxtWepMGRAtk;

        private CustomButton m_SelectedButton;

        void Start()
        {
            if(Debug.isDebugBuild)
            {
                GameStateManager.GetInstance().GetCharacter(0);
                GameStateManager.GetInstance().GetCharacter(1);
            }

            int btnId = 0;
            foreach(Character chara in GameStateManager.GetInstance().Characters)
            {
                if (m_BtnCharacters.Length <= btnId)
                    break;

                m_BtnCharacters[btnId].GetComponentInChildren<Text>().text = chara.m_Name;
                int charaId = chara.m_Id;
                m_BtnCharacters[btnId].onSelect.AddListener(() => { ShowCharacteristics(charaId); });

                ++btnId;
            }

            for (int i = btnId; i < m_BtnCharacters.Length; ++i)
                m_BtnCharacters[btnId].gameObject.SetActive(false);

            Select(m_BtnCharacters[0]);
        }

        void ShowCharacteristics(int _charaId)
        {
            Character selectedCharacter = GameStateManager.GetInstance().GetCharacter(_charaId);
            m_TxtAtk.text = selectedCharacter.m_Statistics.m_Atk.ToString();
            m_TxtDef.text = selectedCharacter.m_Statistics.m_Def.ToString();
            m_TxtMGAtk.text = selectedCharacter.m_Statistics.m_MGAtk.ToString();
            m_TxtMGDef.text = selectedCharacter.m_Statistics.m_MGDef.ToString();
            m_TxtHP.text = string.Format("{0} / {1}", selectedCharacter.m_Statistics.m_HP, selectedCharacter.m_Statistics.m_MaxHP);
            m_TxtMP.text = string.Format("{0} / {1}", selectedCharacter.m_Statistics.m_MP, selectedCharacter.m_Statistics.m_MaxMP);
        }

        void Select(CustomButton _button)
        {
            UnselectedAll();

            m_SelectedButton = _button;
            m_SelectedButton.Select();
        }

        void UnselectedAll()
        {
            foreach (CustomButton btn in m_BtnCharacters)
            {
                if (btn.gameObject.activeInHierarchy && btn.enabled)
                    btn.Deselect();
            }
        }
    }
}
