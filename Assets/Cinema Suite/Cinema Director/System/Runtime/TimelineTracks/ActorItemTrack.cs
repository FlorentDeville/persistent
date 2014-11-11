// Cinema Suite
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    /// <summary>
    /// A track which maintains ActorEvents and ActorActions
    /// </summary>
    public class ActorItemTrack : ActorTrack
    {
        /// <summary>
        /// Initialize the state of all events and actions
        /// </summary>
        public override void Initialize()
        {
            base.elapsedTime = -1f;

            foreach (CinemaActorEvent cinemaEvent in this.ActorEvents)
            {
                foreach (Transform actor in Actors)
                {
                    if (actor != null)
                        cinemaEvent.Initialize(actor.gameObject);
                }
            }

            foreach (CinemaActorAction action in ActorActions)
            {
                //foreach (Transform actor in Actors)
                {
                    action.Initialize();
                }
            }
        }

        public override void SetTime(float time)
        {
            foreach (CinemaActorEvent cinemaEvent in this.ActorEvents)
            {
                float deltaTime = time - cinemaEvent.Firetime;
                cinemaEvent.SetTimeTo(deltaTime);
            }
        }

        public override void Pause()
        {
            foreach (CinemaActorEvent cinemaEvent in this.ActorEvents)
            {
                cinemaEvent.Pause();
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            float previousTime = base.elapsedTime;
            base.elapsedTime = time;
            foreach (CinemaActorEvent cinemaEvent in ActorEvents)
            {
                if ((previousTime < cinemaEvent.Firetime) && (((base.elapsedTime >= cinemaEvent.Firetime))))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                            cinemaEvent.Trigger(actor.gameObject);
                    }
                }
                if (((previousTime >= cinemaEvent.Firetime) && (base.elapsedTime < cinemaEvent.Firetime)))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                            cinemaEvent.Reverse(actor.gameObject);
                    }
                }
            }

            foreach (CinemaActorAction action in ActorActions)
            {
                if ((previousTime < action.Firetime && base.elapsedTime > action.Firetime) && base.elapsedTime < (action.Firetime + action.Duration))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                        {
                            action.Trigger(actor.gameObject);
                        }
                    }
                }
                else if ((base.elapsedTime > action.Firetime) && (base.elapsedTime <= (action.Firetime + action.Duration)))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                        {
                            float runningTime = time - action.Firetime;
                            action.UpdateTime(actor.gameObject, runningTime, deltaTime);
                        }
                    }
                }
                else if (((previousTime <= (action.Firetime + action.Duration)) && (base.elapsedTime > (action.Firetime + action.Duration))))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                        {
                            action.End(actor.gameObject);
                        }
                    }
                }
                //else if (previousTime > action.Firetime && previousTime <= (action.Firetime + action.Duration) && base.elapsedTime < action.Firetime)
                //{
                //    action.ReverseTrigger();
                //}
                //else if ((previousTime > (action.Firetime + action.Duration) && (base.elapsedTime > action.Firetime) && (base.elapsedTime <= (action.Firetime + action.Duration))))
                //{
                //    action.ReverseEnd();
                //}
            }
        }

        /// <summary>
        /// Resume playback after being paused.
        /// </summary>
        public override void Resume()
        {
            foreach (CinemaActorAction cinemaAction in this.ActorActions)
            {
                if (((base.Cutscene.RunningTime > cinemaAction.Firetime)) && (base.Cutscene.RunningTime < (cinemaAction.Firetime + cinemaAction.Duration)))
                {
                    foreach (Transform actor in Actors)
                    {
                        if (actor != null)
                        {
                            cinemaAction.Resume(actor.gameObject);
                        }
                    }
                }
            }
        }

        public override void Stop()
        {
            base.elapsedTime = 0f;
            foreach (CinemaActorEvent actorEvents in this.ActorEvents)
            {
                foreach (Transform actor in Actors)
                {
                    if (actor != null)
                        actorEvents.Stop(actor.gameObject);
                }
            }

            foreach (CinemaActorAction action in ActorActions)
            {
                foreach (Transform actor in Actors)
                {
                    if (actor != null)
                        action.Stop(actor.gameObject);
                }
            }
        }

        public List<Transform> Actors
        {
            get
            {
                ActorTrackGroup trackGroup = TrackGroup as ActorTrackGroup;
                if (trackGroup != null)
                {
                    List<Transform> actors = new List<Transform>() { };
                    actors.Add(trackGroup.Actor);
                    return actors;
                }

                MultiActorTrackGroup multiActorTrackGroup = TrackGroup as MultiActorTrackGroup;
                if (multiActorTrackGroup != null)
                {
                    return multiActorTrackGroup.Actors;
                }
                return null;
            }
        }

        public CinemaActorEvent[] ActorEvents
        {
            get
            {
                return base.GetComponentsInChildren<CinemaActorEvent>();
            }
        }

        public CinemaActorAction[] ActorActions
        {
            get
            {
                return base.GetComponentsInChildren<CinemaActorAction>();
            }
        }
    }
}