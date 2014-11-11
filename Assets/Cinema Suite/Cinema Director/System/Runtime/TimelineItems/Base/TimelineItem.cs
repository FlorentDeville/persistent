using UnityEngine;

namespace CinemaDirector
{
    [ExecuteInEditMode]
    public abstract class TimelineItem : MonoBehaviour
    {
        [SerializeField]
        protected float firetime = -1f;

        /// <summary>
        /// The firetime for this timeline item
        /// </summary>
        public float Firetime
        {
            get { return this.firetime; }
            set
            {
                firetime = value;
                if (firetime < 0f)
                {
                    firetime = 0f;
                }
            }
        }

        /// <summary>
        /// Called when a cutscene begins or enters preview mode.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Called when a cutscene ends or exits preview mode.
        /// </summary>
        public virtual void Stop() { }

        /// <summary>
        /// The cutscene that this timeline item is associated with
        /// </summary>
        public Cutscene Cutscene
        {
            get { return ((this.TimelineTrack == null) ? null : this.TimelineTrack.Cutscene); }
        }

        /// <summary>
        /// The track that this timeline item is associated with
        /// </summary>
        public TimelineTrack TimelineTrack
        {
            get
            {
                if (base.transform.parent == null)
                {
                    return null;
                }
                TimelineTrack component = base.transform.parent.GetComponent<TimelineTrack>();
                if (component == null)
                {

                }
                return component;
            }
        }
    }
}