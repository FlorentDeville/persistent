using Assets.Scripts.UI;

using UnityEngine;

#pragma warning disable 649

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasOptionsBehavior : MonoBehaviour
    {
        [SerializeField]
        private CustomButton m_DefaultSelected;

        void Start()
        {
            m_DefaultSelected.Send(WidgetEvent.Select);
        }
    }
}
