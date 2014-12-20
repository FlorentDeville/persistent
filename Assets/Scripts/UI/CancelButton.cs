using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
 
[System.Serializable]
public class CancelButton : Button, ICancelHandler
{
    public UnityEvent onCancel;

    public UnityEvent onSelect;

    protected override void Awake()
    {
        onCancel = new UnityEvent();
        onSelect = new UnityEvent();
        base.Awake();
    }

    void ICancelHandler.OnCancel(BaseEventData eventData)
    {
        onCancel.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        onSelect.Invoke();
        base.OnSelect(eventData);
    }
}
