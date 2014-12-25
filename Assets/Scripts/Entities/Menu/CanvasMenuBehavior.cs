using Assets.Scripts.Helper;
using Assets.Scripts.Manager;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasMenuBehavior : MonoBehaviour
    {
#pragma warning disable 649

        [SerializeField]
        private ButtonCanvasPair[] m_Tabs;

        [SerializeField]
        private int  m_DefaultTabId;

        private Cooldown m_InputCooldown;

        private int m_SelectedTabId;

        private bool m_StartButtonDown;

        void Awake()
        {
            m_StartButtonDown = false;
            m_InputCooldown = new Cooldown(0.2f);
        }

        void Start()
        {
            //Select and show the characteristics tab by default
            m_Tabs[m_DefaultTabId].m_Btn.Select();
            ShowTab(m_Tabs[m_DefaultTabId].m_Canvas);

            m_SelectedTabId = m_DefaultTabId;
            m_InputCooldown.StartCooldown();
        }

        void Update()
        {
            if (!m_InputCooldown.IsCooldownElapsed())
                return;

            if(Input.GetButton("RB"))
            {
                ++m_SelectedTabId;
                if (m_SelectedTabId >= m_Tabs.Length)
                    m_SelectedTabId = 0;

                UpdateSelectedTab(m_Tabs[m_SelectedTabId]);
                m_InputCooldown.StartCooldown();
            }
            else if(Input.GetButton("LB"))
            {
                --m_SelectedTabId;
                if (m_SelectedTabId < 0)
                    m_SelectedTabId = m_Tabs.Length - 1;

                UpdateSelectedTab(m_Tabs[m_SelectedTabId]);
                m_InputCooldown.StartCooldown();
            }
            else if(Input.GetButtonDown("Start"))
            {
                m_StartButtonDown = true;
            }
            else if(Input.GetButtonUp("Start"))
            {
                if (m_StartButtonDown)
                    GameSceneManager.GetInstance().Pop(true);
            }
        }

        void UpdateSelectedTab(ButtonCanvasPair _selectedTab)
        {
            _selectedTab.m_Btn.Select();
            ShowTab(_selectedTab.m_Canvas);

        }

        void ShowTab(Canvas _tabCanvas)
        {
            HideAllTab();
            _tabCanvas.gameObject.SetActive(true);
        }

        void HideAllTab()
        {
            foreach (ButtonCanvasPair tabCanvas in m_Tabs)
                tabCanvas.m_Canvas.gameObject.SetActive(false);
        }
    }

    [System.Serializable]
    public class ButtonCanvasPair
    {
        public Button m_Btn;
        public Canvas m_Canvas;
    }
}
