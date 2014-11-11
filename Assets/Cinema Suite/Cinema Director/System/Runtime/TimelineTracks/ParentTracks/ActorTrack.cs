using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// A track that is devoted solely to one Actor.
    /// </summary>
    public abstract class ActorTrack : TimelineTrack
    {
        /// <summary>
        /// The Actor that is focused by this Track's ActorTrackGroup. Can return null if no Actor has been set.
        /// </summary>
        public Transform Actor
        {
            get
            {
                ActorTrackGroup component = base.transform.parent.GetComponent<ActorTrackGroup>();
                if (component == null)
                {
                    return null;
                }
                return component.Actor;
            }
        }
    }
}