using System;
using System.Collections.Generic;

using UnityEngine;


namespace Assets.Scripts.UI
{
    public class IWidget : MonoBehaviour
    {
        private Dictionary<WidgetEvent, List<Action>> m_Dispatch;

        protected bool m_HasFocus;

        protected void Awake()
        {
            m_HasFocus = false;
            m_Dispatch = new Dictionary<WidgetEvent, List<Action>>();
        }

        public void Connect(WidgetEvent _event, Action _act)
        {
            if (!m_Dispatch.ContainsKey(_event))
                m_Dispatch.Add(_event, new List<Action>());

            List<Action> actionsList = m_Dispatch[_event];
            actionsList.Add(_act);
        }

        public void Send(WidgetEvent _event)
        {
            if (m_Dispatch == null || !m_Dispatch.ContainsKey(_event))
                return;

            List<Action> actionsList = m_Dispatch[_event];
            foreach (Action act in actionsList)
                act.Invoke();
        }

        public bool HasFocus() { return m_HasFocus; }
    }
}
