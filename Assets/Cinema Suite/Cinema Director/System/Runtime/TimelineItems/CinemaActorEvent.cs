// Cinema Suite
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The base class for all cinema events
    /// </summary>
    [ExecuteInEditMode]
    public abstract class CinemaActorEvent : TimelineItem
    {
        public abstract void Trigger(GameObject Actor);

        public virtual void Reverse(GameObject Actor) { }

        public virtual void SetTimeTo(float deltaTime) { }

        public virtual void Pause() { }
        public virtual void Resume() { }

        public virtual void Initialize(GameObject Actor) { }
        public virtual void Stop(GameObject Actor) { }

        public ActorTrackGroup ActorTrackGroup
        {
            get
            {
                return this.TimelineTrack.TrackGroup as ActorTrackGroup;
            }
        }
    }
}