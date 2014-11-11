using UnityEngine;
namespace CinemaDirector
{
    /// <summary>
    /// The base Track that all others should inherit.
    /// </summary>
    public abstract class TimelineTrack : MonoBehaviour
    {
        [SerializeField]
        private int ordinal = -1; // Used to order within a track group.

        protected float elapsedTime; // The maount of time that has elapsed so far

        /// <summary>
        /// Perform any initialization before the cutscene begins a fresh playback
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Update the track to the given time
        /// </summary>
        /// <param name="runningTime">The current running time of the cutscene.</param>
        /// /// <param name="deltaTime">The time that has passed since the last update call.</param>
        public virtual void UpdateTrack(float runningTime, float deltaTime) { }

        /// <summary>
        /// Notify track items that the cutscene has been paused
        /// </summary>
        public virtual void Pause() { }

        /// <summary>
        /// Notify track itehsm that the cutscene has been resumed
        /// </summary>
        public virtual void Resume() { }

        /// <summary>
        /// The cutscene has been set to an arbitrary time by the user.
        /// </summary>
        /// <param name="time">The new cutscene running time</param>
        public virtual void SetTime(float time) { }

        /// <summary>
        /// Notify the track items that the cutscene has been stopped
        /// </summary>
        public virtual void Stop() { }

        /// <summary>
        /// The cutscene associated with this Track. Can return null.
        /// </summary>
        public Cutscene Cutscene
        {
            get
            {
                return this.TrackGroup.Cutscene;
            }
        }

        /// <summary>
        /// The TrackGroup associated with this Track. Can return null.
        /// </summary>
        public TrackGroup TrackGroup
        {
            get
            {
                return transform.parent.GetComponent<TrackGroup>();
            }
        }

        /// <summary>
        /// The ordinal of this Track within its TrackGroup.
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

        /// <summary>
        /// The timeline items that this track maintains as children.
        /// </summary>
        public virtual TimelineItem[] TimelineItems
        {
            get { return base.GetComponentsInChildren<TimelineItem>(); }
        }
    }
}