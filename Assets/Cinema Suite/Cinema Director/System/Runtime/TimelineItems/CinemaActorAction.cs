// Cinema Suite
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// The base class for all cinema events
    /// </summary>
    [ExecuteInEditMode]
    public abstract class CinemaActorAction : TimelineAction
    {

        /// <summary>
        /// Called when the running time of the cutscene hits the firetime of the action
        /// </summary>
        public abstract void Trigger(GameObject Actor);

        /// <summary>
        /// Called at each update when the action is to be played.
        /// </summary>
        public virtual void UpdateTime(GameObject Actor, float time, float deltaTime) { }

        /// <summary>
        /// Called when the running time of the cutscene exceeds the duration of the action
        /// </summary>
        public abstract void End(GameObject Actor);

        /// <summary>
        /// Called when the cutscene exists preview/play mode. Return properties to pre-cached state if necessary.
        /// </summary>
        public virtual void Stop(GameObject Actor) { }

        /// <summary>
        /// Called when the cutscene time is set/skipped manually.
        /// </summary>
        public virtual void SetTime(float time, float deltaTime) { }

        /// <summary>
        /// Pause any action as necessary
        /// </summary>
        public virtual void Pause(GameObject Actor) { }

        /// <summary>
        /// Resume from paused.
        /// </summary>
        public virtual void Resume(GameObject Actor) { }

        /// <summary>
        /// Reverse trigger. Called when scrubbing backwards.
        /// </summary>
        public virtual void ReverseTrigger() { }

        /// <summary>
        /// Reverse End. Called when scrubbing backwards.
        /// </summary>
        public virtual void ReverseEnd() { }

        public int CompareTo(object other)
        {
            CinemaGlobalAction otherAction = (CinemaGlobalAction)other;
            return (int)(otherAction.Firetime - this.Firetime);
        }
    }
}