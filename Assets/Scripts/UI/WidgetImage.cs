using UnityEngine.Events;

namespace Assets.Scripts.UI
{
    public class WidgetImage : IWidget
    {
        public UnityEvent onCancel;

        new void Awake()
        {
            base.Awake();
            Connect(WidgetEvent.Cancel, OnCancel);
        }

        void OnCancel()
        {
            onCancel.Invoke();
        }
    }
}
