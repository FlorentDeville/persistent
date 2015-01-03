using Assets.Scripts.Assets;
using Assets.Scripts.UI;
using Assets.Scripts.Entities.World;
using Assets.Scripts.Manager;

using System;

using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 649

namespace Assets.Scripts.Entities.UI
{
    public class CombatUI_MagicList : MonoBehaviour
    {
        [SerializeField]
        private WidgetImage m_SelectedMagic;

        [SerializeField]
        private GameObject m_ListContainer;

        private Action m_OnClose;
        public Action OnClose
        {
            get { return m_OnClose; }
            set { m_OnClose = value; }
        }

        private Action<MagicId> m_OnClicked;
        public Action<MagicId> OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        CustomButton[] m_BtnMagic;

        void Awake()
        {
            m_BtnMagic = m_ListContainer.GetComponentsInChildren<CustomButton>(true);
        }

        public void InitializeMagicList(MagicId[] _availableMagic)
        {
            m_ListContainer.SetActive(true);
            m_SelectedMagic.gameObject.SetActive(false);

            m_SelectedMagic.onCancel.RemoveAllListeners();
            m_SelectedMagic.onCancel.AddListener(() => { ShowMagicList(); });

            int btnId = 0;
            foreach(MagicId id in _availableMagic)
            {
                if (btnId == m_BtnMagic.Length)
                    break;

                MagicDescription desc = MagicManager.GetInstance().GetDescription(id);
                if(desc == null)
                {
                    Debug.LogError(string.Format("Can't find magic description with id {0}", id));
                    continue;
                }

                CustomButton btn = m_BtnMagic[btnId];

                btn.GetComponentInChildren<Text>().text = desc.m_DisplayName;

                btn.onCancel.RemoveAllListeners();
                btn.onCancel.AddListener(() => { Close(); });

                MagicId capturedId = id; //necessary for the lambda to work
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => { Clicked(capturedId); });

                btn.gameObject.SetActive(true);

                PawnStatistics stats = GameTurnManager.GetInstance().GetCurrentPawnStatistics();
                if (desc.m_Power.CanBeUsed(stats))
                    btn.IsUnselectable = false;
                else
                    btn.IsUnselectable = true;
                
                btn.Send(WidgetEvent.Unselect);
                ++btnId;
            }

            for (int i = btnId; i < m_BtnMagic.Length; ++i)
                m_BtnMagic[i].gameObject.SetActive(false);

            m_BtnMagic[0].Send(WidgetEvent.Select);            
        }

        void Close()
        {
            WidgetManager.GetInstance().Hide();
            if (m_OnClose != null)
                OnClose();
        }

        public void ShowMagicList()
        {
            m_ListContainer.SetActive(true);
            m_SelectedMagic.gameObject.SetActive(false);
        }

        void Clicked(MagicId _clickedMagicId)
        {
            m_ListContainer.SetActive(false);
            m_SelectedMagic.gameObject.SetActive(true);
            m_SelectedMagic.GetComponentInChildren<Text>().text = MagicManager.GetInstance().GetDescription(_clickedMagicId).m_DisplayName;
            m_SelectedMagic.SetFocus();

            if (m_OnClicked != null)
                m_OnClicked(_clickedMagicId);
        }
    }
}
