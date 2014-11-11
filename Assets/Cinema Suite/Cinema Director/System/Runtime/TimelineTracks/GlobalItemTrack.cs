using UnityEngine;

namespace CinemaDirector
{
    public class GlobalItemTrack : GlobalTrack
    {
        public override void Initialize()
        {
            base.elapsedTime = -1f;

            foreach (TimelineItem item in TimelineItems)
            {
                item.Initialize();
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            float previousTime = base.elapsedTime;
            base.elapsedTime = time;

            foreach (CinemaGlobalEvent cinemaEvent in Events)
            {
                if ((previousTime < cinemaEvent.Firetime) && (((base.elapsedTime >= cinemaEvent.Firetime))))
                {
                    cinemaEvent.Trigger();
                }
                if (((previousTime >= cinemaEvent.Firetime) && (base.elapsedTime < cinemaEvent.Firetime)))
                {
                    cinemaEvent.Reverse();
                }
            }

            foreach (CinemaGlobalAction action in Actions)
            {
                if ((previousTime < action.Firetime && base.elapsedTime > action.Firetime) && base.elapsedTime < (action.Firetime + action.Duration))
                {
                    action.Trigger();
                }
                else if ((base.elapsedTime > action.Firetime) && (base.elapsedTime < (action.Firetime + action.Duration)))
                {
                    float runningTime = time - action.Firetime;
                    action.UpdateTime(runningTime, deltaTime);
                }
                else if (((previousTime < (action.Firetime + action.Duration)) && (base.elapsedTime >= (action.Firetime + action.Duration))))
                {
                    action.End();
                }
                else if (previousTime > action.Firetime && previousTime <= (action.Firetime + action.Duration) && base.elapsedTime < action.Firetime)
                {
                    action.ReverseTrigger();
                }
                else if ((previousTime > (action.Firetime + action.Duration) && (base.elapsedTime > action.Firetime) && (base.elapsedTime <= (action.Firetime + action.Duration))))
                {
                    action.ReverseEnd();
                }
            }
        }

        public override void SetTime(float time)
        {
            float deltaTime = time - base.elapsedTime;
            foreach (CinemaGlobalAction action in Actions)
            {
                float runningTime = time - action.Firetime;
                action.SetTime(runningTime, deltaTime);
            }
        }

        public override void Stop()
        {
            foreach (TimelineItem item in TimelineItems)
            {
                item.Stop();
            }
        }

        public CinemaGlobalEvent[] Events
        {
            get
            {
                return base.GetComponentsInChildren<CinemaGlobalEvent>();
            }
        }

        public CinemaGlobalAction[] Actions
        {
            get
            {
                return base.GetComponentsInChildren<CinemaGlobalAction>();
            }
        }

        public override TimelineItem[] TimelineItems
        {
            get
            {
                CinemaGlobalEvent[] events = Events;
                CinemaGlobalAction[] actions = Actions;

                TimelineItem[] items = new TimelineItem[events.Length + actions.Length];
                for (int i = 0; i < events.Length; i++)
                {
                    items[i] = events[i];
                }

                for (int i = 0; i < actions.Length; i++)
                {
                    items[i + events.Length] = actions[i];
                }

                return items;
            }
        }
    }
}