using Assets.Scripts.Helper;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class CustomButton : MonoBehaviour
    {
#pragma warning disable 649
        public enum CustomButtonState
        {
            Selected,
            Unselected,
            Unknown
        }

        public Color m_Normal;

        public Color m_Highlighted;

        public string Text
        {
            get{ return m_Text; }
            set
            {
                m_Text = value;
                if (m_TextWidget != null)
                    m_TextWidget.text = m_Text;
            }
        }
        
        public UnityEvent onSelect;

        public UnityEvent onDeselect;

        private Image m_ImageWidget;

        private Text m_TextWidget;

        private CustomButtonState m_State;

        [SerializeField]
        private string m_Text;

        [SerializeField]
        private CustomButton m_Top;

        [SerializeField]
        private CustomButton m_Bottom;

        private Cooldown m_InputCooldown;

        void Awake()
        {
            m_InputCooldown = new Cooldown(0.2f);
            m_ImageWidget = GetComponent<Image>();
            m_TextWidget = GetComponentInChildren<Text>();
            m_TextWidget.text = m_Text;
            Deselect();
        }

        public void Select()
        {
            m_State = CustomButtonState.Selected;
            if (m_ImageWidget != null)
                m_ImageWidget.color = m_Highlighted;
            onSelect.Invoke();
            m_InputCooldown.StartCooldown();
        }

        public void Deselect()
        {
            m_State = CustomButtonState.Unselected;
            if(m_ImageWidget != null)
                m_ImageWidget.color = m_Normal;
            onDeselect.Invoke();
        }

        void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            if (m_State != CustomButtonState.Selected)
                return;

            if (!m_InputCooldown.IsCooldownElapsed())
                return;

            if(Input.GetAxis("Vertical") > 0.9f)
            {
                if(m_Top != null && m_Top.enabled && m_Top.gameObject.activeInHierarchy)
                {
                    Deselect();
                    m_Top.Select();
                }
            }
            else if(Input.GetAxis("Vertical") < -0.9f)
            {
                if(m_Bottom != null && m_Bottom.enabled && m_Bottom.gameObject.activeInHierarchy)
                {
                    Deselect();
                    m_Bottom.Select();
                }
            }
        }
    }
}
