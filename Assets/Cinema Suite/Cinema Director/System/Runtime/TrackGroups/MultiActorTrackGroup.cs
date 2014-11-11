using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The MultiActorTrackGroup maintains a list of Actors that have something in 
    /// common and a set of tracks that act upon those Actors.
    /// </summary>
    public class MultiActorTrackGroup : TrackGroup
    {
        [SerializeField]
        private List<Transform> actors = new List<Transform>();

        /// <summary>
        /// The Actors that this TrackGroup is focused on
        /// </summary>
        public List<Transform> Actors
        {
            get
            {
                return actors;
            }
            set
            {
                actors = value;
            }
        }

        /// <summary>
        /// Get the TimelineTracks associated with this TrackGroup.
        /// </summary>
        /// <returns>A set of Tracks that this TrackGroup is a parent of.</returns>
        public override TimelineTrack[] GetTracks()
        {
            return GetComponentsInChildren<TimelineTrack>();
        }
    }
}
