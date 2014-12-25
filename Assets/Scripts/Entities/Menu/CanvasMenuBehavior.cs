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

        [SerializeField]
        private float m_InputCooldown;

        private int m_SelectedTabId;

        private float m_LastInputTime;

        private bool m_StartButtonDown;

        void Awake()
        {
            m_StartButtonDown = false;
        }

        void Start()
        {
            //Select and show the characteristics tab by default
            m_Tabs[m_DefaultTabId].m_Btn.Select();
            ShowTab(m_Tabs[m_DefaultTabId].m_Canvas);

            m_SelectedTabId = m_DefaultTabId;
            m_LastInputTime = 0;
        }

        void Update()
        {
            if (!CanTakeInput())
                return;

            if(Input.GetButton("RB"))
            {
                m_LastInputTime = Time.fixedTime;
                ++m_SelectedTabId;
                if (m_SelectedTabId >= m_Tabs.Length)
                    m_SelectedTabId = 0;

                UpdateSelectedTab(m_Tabs[m_SelectedTabId]);
            }
            else if(Input.GetButton("LB"))
            {
                m_LastInputTime = Time.fixedTime;
                --m_SelectedTabId;
                if (m_SelectedTabId < 0)
                    m_SelectedTabId = m_Tabs.Length - 1;

                UpdateSelectedTab(m_Tabs[m_SelectedTabId]);
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

        bool CanTakeInput()
        {
            if (m_LastInputTime + m_InputCooldown < Time.fixedTime)
                return true;

            return false;
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
