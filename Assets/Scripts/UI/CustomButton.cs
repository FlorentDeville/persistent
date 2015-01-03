using Assets.Scripts.Helper;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class CustomButton : IWidget
    {
#pragma warning disable 649
        public enum CustomButtonState
        {
            Selected,
            Unselected,
            Pressed,
            Unknown
        }

        public Color m_Normal;

        public Color m_Highlighted;

        public Color m_Pressed;

        public Color m_UnselectableNormal;

        public Color m_UnselectableHighlighted;

        [SerializeField]
        private bool m_IsUnselectable;
        public bool IsUnselectable
        {
            get { return m_IsUnselectable; }
            set
            {
                if (m_IsUnselectable == value) return;

                OnIsUnselectableChanged(value);
            }
        }

        public UnityEvent onSelect;

        public UnityEvent onDeselect;

        public UnityEvent onClick;

        public UnityEvent onCancel;

        private Image m_ImageWidget;

        private CustomButtonState m_State;

        [SerializeField]
        private CustomButton m_Top;

        [SerializeField]
        private CustomButton m_Bottom;

        [SerializeField]
        private CustomButton m_Left;

        [SerializeField]
        private CustomButton m_Right;

        private Cooldown m_InputCooldown;

        new void Awake()
        {
            base.Awake();

            m_InputCooldown = new Cooldown(0.1f);
            m_ImageWidget = GetComponent<Image>();
            OnDeselect();

            Connect(WidgetEvent.Select, OnSelect);
            Connect(WidgetEvent.Unselect, OnDeselect);
            Connect(WidgetEvent.Up, () => { SelectOther(m_Top); });
            Connect(WidgetEvent.Down, () => { SelectOther(m_Bottom); });
            Connect(WidgetEvent.Left, () => { SelectOther(m_Left); });
            Connect(WidgetEvent.Right, () => { SelectOther(m_Right); });
            Connect(WidgetEvent.Submit, OnSumbit);
            Connect(WidgetEvent.Cancel, OnCancel);
        }

        private void OnSelect()
        {
            m_HasFocus = true;
            m_State = CustomButtonState.Selected;
            UpdateColor();
            onSelect.Invoke();
            m_InputCooldown.StartCooldown();
        }

        private void OnDeselect()
        {
            m_HasFocus = false;
            m_State = CustomButtonState.Unselected;
            UpdateColor();
            onDeselect.Invoke();
        }

        private void SelectOther(CustomButton _btn)
        {
            if (m_State != CustomButtonState.Selected)
                return;

            if(_btn != null && _btn.enabled && _btn.gameObject.activeInHierarchy)
            {
                OnDeselect();
                _btn.Send(WidgetEvent.Select);//Select();
            }
        }

        private void OnSumbit()
        {
            if (m_State != CustomButtonState.Selected 
                || m_IsUnselectable
                )
                return;

            m_State = CustomButtonState.Pressed;
            UpdateColor();
            m_InputCooldown.StartCooldown();
            onClick.Invoke();
        }

        private void OnCancel()
        {
            if (m_State != CustomButtonState.Selected)
                return;

            onCancel.Invoke();
        }

        private void OnIsUnselectableChanged(bool _value)
        {
            m_IsUnselectable = _value;
            UpdateColor();
        }

        private void UpdateColor()
        {
            if (m_ImageWidget == null) return;
            switch(m_State)
            {
                case CustomButtonState.Pressed:
                    m_ImageWidget.color = m_Pressed;
                    break;

                case CustomButtonState.Selected:
                    if (m_IsUnselectable)
                        m_ImageWidget.color = m_UnselectableHighlighted;
                    else
                        m_ImageWidget.color = m_Highlighted;
                    break;

                case CustomButtonState.Unselected:
                    if (m_IsUnselectable)
                        m_ImageWidget.color = m_UnselectableNormal;
                    else
                        m_ImageWidget.color = m_Normal;
                    break;

                default: 
                    break;
            }
        }
        void Update()
        {
            if (m_State == CustomButtonState.Pressed && m_InputCooldown.IsCooldownElapsed())
            {
                    m_State = CustomButtonState.Selected;
                    m_ImageWidget.color = m_Highlighted;
                    return;
            }
        }
    }
}
