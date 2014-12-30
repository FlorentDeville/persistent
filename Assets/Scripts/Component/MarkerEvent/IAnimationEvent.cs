using UnityEngine;

namespace Assets.Scripts.Component.MarkerEvent
{
    public abstract class IAnimationEvent : MonoBehaviour
    {
        [SerializeField]
        protected DisplacementMode m_Mode;

        public abstract void StartEvent();

        public abstract void StopEvent();
    }

    public enum DisplacementMode
    {
        OnSource,
        OnTarget,
        FromPawnToTarget,
        FromTargetToPawn
    }
}
