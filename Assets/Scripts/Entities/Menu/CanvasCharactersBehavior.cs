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

        [SerializeField]
        private float m_InputCooldown;

        private float m_LastInputTime;

        private int m_SelectedButtonId;

        private List<KeyValuePair<int, CustomButton>> m_MapCharaIdButton;

        void Start()
        {
            m_LastInputTime = 0;
            m_MapCharaIdButton = new List<KeyValuePair<int,CustomButton>>(m_BtnCharacters.Length);

            if(Debug.isDebugBuild)
            {
                GameStateManager.GetInstance().GetCharacter(0);
                GameStateManager.GetInstance().GetCharacter(1);
            }

            m_SelectedButtonId = 0;
            int btnId = 0;
            foreach(Character chara in GameStateManager.GetInstance().Characters)
            {
                if (m_BtnCharacters.Length <= btnId)
                    break;

                m_MapCharaIdButton.Add(new KeyValuePair<int, CustomButton>(chara.m_Id, m_BtnCharacters[btnId]));

                m_BtnCharacters[btnId].GetComponentInChildren<Text>().text = chara.m_Name;
                m_BtnCharacters[btnId].onSelect.AddListener(() =>
                    {
                        int charaId = m_MapCharaIdButton[m_SelectedButtonId].Key;
                        Character selectedCharacter = GameStateManager.GetInstance().GetCharacter(charaId);
                        ShowCharacteristics(selectedCharacter);
                    });

                ++btnId;
            }

            for (int i = btnId; i < m_BtnCharacters.Length; ++i)
                m_BtnCharacters[btnId].gameObject.SetActive(false);

            Select(0);
        }

        void Update()
        {
            if (!CanTakeInput())
                return;

            if(Input.GetAxis("Vertical") > 0.9f)
            {
                --m_SelectedButtonId;
                if (m_SelectedButtonId < 0)
                    m_SelectedButtonId = 0;

                Select(m_SelectedButtonId);
                m_LastInputTime = Time.fixedTime;
            }
            else if(Input.GetAxis("Vertical") < -0.9f)
            {
                ++m_SelectedButtonId;
                if (m_SelectedButtonId >= m_MapCharaIdButton.Count)
                    m_SelectedButtonId = m_MapCharaIdButton.Count - 1;

                Select(m_SelectedButtonId);
                m_LastInputTime = Time.fixedTime;
            }
        }

        bool CanTakeInput()
        {
            if (m_LastInputTime + m_InputCooldown < Time.fixedTime)
                return true;

            return false;
        }

        void ShowCharacteristics(Character _chara)
        {
            m_TxtAtk.text = _chara.m_Statistics.m_Atk.ToString();
            m_TxtDef.text = _chara.m_Statistics.m_Def.ToString();
            m_TxtMGAtk.text = _chara.m_Statistics.m_MGAtk.ToString();
            m_TxtMGDef.text = _chara.m_Statistics.m_MGDef.ToString();
            m_TxtHP.text = string.Format("{0} / {1}", _chara.m_Statistics.m_HP, _chara.m_Statistics.m_MaxHP);
            m_TxtMP.text = string.Format("{0} / {1}", _chara.m_Statistics.m_MP, _chara.m_Statistics.m_MaxMP);
        }

        void Select(int _buttonId)
        {
            UnselectedAll();

            m_SelectedButtonId = _buttonId;
            m_MapCharaIdButton[_buttonId].Value.Select();
        }

        void UnselectedAll()
        {
            foreach (KeyValuePair<int, CustomButton> pair in m_MapCharaIdButton)
                pair.Value.Deselect();
        }
    }
}
