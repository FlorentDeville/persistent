using System;
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The ActorTrackGroup maintains an Actor and a set of tracks that affect 
    /// that actor over the course of the Cutscene.
    /// </summary>
    public class ActorTrackGroup : TrackGroup
    {
        [SerializeField]
        private Transform actor;

        /// <summary>
        /// The Actor that this TrackGroup is focused on.
        /// </summary>
        public Transform Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// Get the ActorTracks that this TrackGroup contains.
        /// </summary>
        /// <returns>A set of ActorTracks that this TrackGroup is a parent of.</returns>
        public override TimelineTrack[] GetTracks()
        {
            return GetComponentsInChildren<ActorTrack>();
        }
    }
}