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

        [SerializeField]
        private bool m_HandleInput;
        public bool HandleInput
        {
            get { return m_HandleInput; }
            set
            {
                m_HandleInput = value;
                if (m_HandleInput)
                    m_InputCooldown.StartCooldown();
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

        void Awake()
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
            if (m_ImageWidget != null)
                m_ImageWidget.color = m_Highlighted;
            onSelect.Invoke();
            m_InputCooldown.StartCooldown();
        }

        private void OnDeselect()
        {
            m_HasFocus = false;
            m_State = CustomButtonState.Unselected;
            if(m_ImageWidget != null)
                m_ImageWidget.color = m_Normal;
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
            if (m_State != CustomButtonState.Selected)
                return;

            m_State = CustomButtonState.Pressed;
            m_ImageWidget.color = m_Pressed;
            m_InputCooldown.StartCooldown();
            onClick.Invoke();
        }

        private void OnCancel()
        {
            if (m_State != CustomButtonState.Selected)
                return;

            onCancel.Invoke();
        }

        void Update()
        {
            if (m_State == CustomButtonState.Pressed && m_InputCooldown.IsCooldownElapsed())
            {
                    m_State = CustomButtonState.Selected;
                    m_ImageWidget.color = m_Highlighted;
                    return;
            }
            //if (m_HandleInput)
            //    UpdateInput();
        }

        private void UpdateInput()
        {
            //if (m_State == CustomButtonState.Unselected)
            //    return;

            //if(m_State == CustomButtonState.Pressed)
            //{
            //    if(m_InputCooldown.IsCooldownElapsed())
            //    {
            //        m_State = CustomButtonState.Selected;
            //        m_ImageWidget.color = m_Highlighted;
            //        return;
            //    }
            //}

            //if (!m_InputCooldown.IsCooldownElapsed())
            //    return;

            //if(Input.GetAxis("Vertical") > 0.9f)
            //{
            //    if(m_Top != null && m_Top.enabled && m_Top.gameObject.activeInHierarchy)
            //    {
            //        Deselect();
            //        m_Top.Select();
            //    }
            //}
            //else if(Input.GetAxis("Vertical") < -0.9f)
            //{
            //    if(m_Bottom != null && m_Bottom.enabled && m_Bottom.gameObject.activeInHierarchy)
            //    {
            //        Deselect();
            //        m_Bottom.Select();
            //    }
            //}
            //else if(Input.GetAxis("Horizontal") > 0.9f)
            //{
            //    if (m_Right != null && m_Right.enabled && m_Right.gameObject.activeInHierarchy)
            //    {
            //        Deselect();
            //        m_Right.Select();
            //    }
            //}
            //else if (Input.GetAxis("Horizontal") < -0.9f)
            //{
            //    if (m_Left != null && m_Left.enabled && m_Left.gameObject.activeInHierarchy)
            //    {
            //        Deselect();
            //        m_Left.Select();
            //    }
            //}
            //else if(Input.GetButton("Submit") && m_State == CustomButtonState.Selected)
            //{
            //    m_State = CustomButtonState.Pressed;
            //    m_ImageWidget.color = m_Pressed;
            //    m_InputCooldown.StartCooldown();
            //    onClick.Invoke();
            //}
            //else if(Input.GetButton("Cancel") && m_State == CustomButtonState.Selected)
            //{
            //    m_InputCooldown.StartCooldown();
            //    onCancel.Invoke();
            //}
        }
    }
}
