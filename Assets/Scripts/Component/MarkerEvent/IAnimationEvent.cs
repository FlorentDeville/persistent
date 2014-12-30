using UnityEngine;

namespace Assets.Scripts.Component.MarkerEvent
{
    public abstract class IAnimationEvent : MonoBehaviour
    {
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
