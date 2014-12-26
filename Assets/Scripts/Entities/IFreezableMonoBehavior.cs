using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class IFreezableMonoBehavior : MonoBehaviour
    {
        protected bool m_Freezed;
        public bool Freezed
        {
            get { return m_Freezed; }
            set
            {
                if(value != m_Freezed)
                {
                    m_Freezed = value;
                    OnFreeze(m_Freezed);
                }
            }
        }

        protected virtual void OnFreeze(bool _value) { }
    }
}
