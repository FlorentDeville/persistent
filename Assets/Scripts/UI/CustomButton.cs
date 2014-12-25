using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class CustomButton : MonoBehaviour
    {
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

        void Start()
        {
            m_ImageWidget = GetComponent<Image>();
            m_TextWidget = GetComponentInChildren<Text>();
            m_TextWidget.text = m_Text;
            Deselect();
        }

        public void Select()
        {
            m_State = CustomButtonState.Selected;
            m_ImageWidget.color = m_Highlighted;
            onSelect.Invoke();
        }

        public void Deselect()
        {
            m_State = CustomButtonState.Unselected;
            m_ImageWidget.color = m_Normal;
            onDeselect.Invoke();
        }
    }
}
