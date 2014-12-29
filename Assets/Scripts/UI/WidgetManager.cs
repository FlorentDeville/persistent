using Assets.Scripts.Helper;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Assets.Scripts.UI
{
    public class WidgetManager : MonoBehaviour
    {
        private Stack<GameObject> m_CanvasStack;

        private IWidget[] m_CurrentWidgets;

        private Cooldown m_InputCooldown;

        private static WidgetManager m_Instance = null;

        public static WidgetManager GetInstance()
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<WidgetManager>();

            return m_Instance;
        }

        public void Show(GameObject _canvas, bool _disablePrevious, bool _unselectPrevious)
        {
            if(_disablePrevious && m_CanvasStack.Count != 0)
            {
                GameObject top = m_CanvasStack.Peek();
                if (top != null)
                    top.SetActive(false);
            }

            if(_unselectPrevious)
                SendToAll(WidgetEvent.Unselect);

            m_CanvasStack.Push(_canvas);
            m_CurrentWidgets = _canvas.transform.GetComponentsInChildren<IWidget>(true);
            _canvas.SetActive(true);
        }

        public GameObject Hide()
        {
            if (m_CanvasStack.Count == 0)
                return null;

            GameObject obj = m_CanvasStack.Pop();
            obj.SetActive(false);

            if (m_CanvasStack.Count != 0)
                m_CurrentWidgets = m_CanvasStack.Peek().transform.GetComponentsInChildren<IWidget>(true);
            else
                m_CurrentWidgets = null;

            return obj;
        }

        public void HideAll()
        {
            while (m_CanvasStack.Count != 0)
                Hide();

            m_CurrentWidgets = null;
        }

        void Awake()
        {
            m_CanvasStack = new Stack<GameObject>();
            m_CurrentWidgets = null;
            m_InputCooldown = new Cooldown(0.2f);
        }

        void Update()
        {
            if (!m_InputCooldown.IsCooldownElapsed())
                return;

            if (Input.GetAxis("Vertical") > 0.9f)
            {
                SendToWidgetsWithFocus(WidgetEvent.Up);
                m_InputCooldown.StartCooldown();
            }
            else if (Input.GetAxis("Vertical") < -0.9f)
            {
                SendToWidgetsWithFocus(WidgetEvent.Down);
                m_InputCooldown.StartCooldown();
            }
            else if (Input.GetAxis("Horizontal") > 0.9f)
            {
                SendToWidgetsWithFocus(WidgetEvent.Right);
                m_InputCooldown.StartCooldown();
            }
            else if (Input.GetAxis("Horizontal") < -0.9f)
            {
                SendToWidgetsWithFocus(WidgetEvent.Left);
                m_InputCooldown.StartCooldown();
            }
            else if (Input.GetButtonDown("Submit"))
            {
                SendToWidgetsWithFocus(WidgetEvent.Submit);
                m_InputCooldown.StartCooldown();
            }
            else if (Input.GetButtonDown("Cancel"))
            {
                SendToWidgetsWithFocus(WidgetEvent.Cancel);
                m_InputCooldown.StartCooldown();
            }
        }

        void SendToAll(WidgetEvent _event)
        {
            if (m_CurrentWidgets == null)
                return;

            foreach(IWidget widget in m_CurrentWidgets)
            {
                if (widget.gameObject.activeInHierarchy && widget.enabled)
                    widget.Send(_event);
            }
        }

        void SendToWidgetsWithFocus(WidgetEvent _event)
        {
            if (m_CurrentWidgets == null)
                return;

            List<IWidget> widgetWithFocus = m_CurrentWidgets.ToList().FindAll(item => item.HasFocus());
            foreach (IWidget widget in widgetWithFocus)
            {
                if (widget.gameObject.activeInHierarchy && widget.enabled)
                {
                    widget.Send(_event);
                }
            }
        }
    }
}
