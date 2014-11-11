using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The main organizational unit of a Cutscene, The TrackGroup contains tracks.
    /// </summary>
    public abstract class TrackGroup : MonoBehaviour
    {
        [SerializeField]
        private int ordinal = -1; // For ordering in UI

        /// <summary>
        /// Initialize all Tracks before beginning a fresh playback.
        /// </summary>
        public virtual void Initialize()
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.Initialize();
            }
        }

        /// <summary>
        /// Update the track group to the current running time of the cutscene.
        /// </summary>
        /// <param name="time">The current running time</param>
        /// <param name="deltaTime">The deltaTime since the last update call</param>
        public virtual void UpdateTrackGroup(float time, float deltaTime)
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.UpdateTrack(time, deltaTime);
            }
        }

        /// <summary>
        /// Pause all Track Items that this TrackGroup contains.
        /// </summary>
        public virtual void Pause()
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.Pause();
            }
        }

        /// <summary>
        /// Stop all Track Items that this TrackGroup contains.
        /// </summary>
        public virtual void Stop()
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.Stop();
            }
        }

        /// <summary>
        /// Resume all Track Items that this TrackGroup contains.
        /// </summary>
        public virtual void Resume()
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.Resume();
            }
        }

        /// <summary>
        /// Set this TrackGroup to the state of a given new running time.
        /// </summary>
        /// <param name="time">The new running time</param>
        public virtual void SetRunningTime(float time)
        {
            foreach (TimelineTrack track in GetTracks())
            {
                track.SetTime(time);
            }
        }

        /// <summary>
        /// The Cutscene that this TrackGroup is associated with. Will return null if TrackGroup does not have a Cutscene as a parent.
        /// </summary>
        public Cutscene Cutscene
        {
            get
            {
                Cutscene cutscene = null;
                if (transform.parent != null)
                {
                    cutscene = transform.parent.GetComponent<Cutscene>();
                }
                return cutscene;
            }
        }

        /// <summary>
        /// The TimelineTracks that this TrackGroup contains.
        /// </summary>
        public abstract TimelineTrack[] GetTracks();

        /// <summary>
        /// Ordinal for UI ranking.
        /// </summary>
        public int Ordinal
        {
            get
            {
                return ordinal;
            }
            set
            {
                ordinal = value;
            }
        }
    }
}