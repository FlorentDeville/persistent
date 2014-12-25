using Assets.Scripts.UI;

using UnityEngine;

namespace Assets.Scripts.Entities.Menu
{
    public class CanvasOptionsBehavior : MonoBehaviour
    {
        [SerializeField]
        private CustomButton m_DefaultSelected;

        void Start()
        {
            m_DefaultSelected.Select();
        }
    }
}
