using UnityEngine;

namespace CinemaDirector
{
    public abstract class TimelineAction : TimelineItem
    {
        [SerializeField]
        protected float duration = 0f;

        /// <summary>
        /// The duration of the action
        /// </summary>
        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}