// Cinema Suite
using UnityEngine;

namespace CinemaDirector
{
    [ExecuteInEditMode]
    public abstract class CinemaGlobalEvent : TimelineItem
    {
        public abstract void Trigger();

        public virtual void Reverse() { }
    }
}