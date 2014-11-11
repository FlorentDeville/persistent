
namespace CinemaDirector
{
    public class CurveTrack : ActorTrack
    {
        public override void Initialize()
        {
            foreach (CinemaActorClipCurve actorClipCurve in this.TimelineItems)
            {
                actorClipCurve.Initialize();
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            base.elapsedTime = time;
            foreach (CinemaActorClipCurve actorClipCurve in this.TimelineItems)
            {
                actorClipCurve.SampleTime(time);
            }
        }

        public override void SetTime(float time)
        {
            base.elapsedTime = time;
            foreach (CinemaActorClipCurve actorClipCurve in this.TimelineItems)
            {
                actorClipCurve.SampleTime(time);
            }
        }

        public override void Stop()
        {
            foreach (CinemaActorClipCurve actorClipCurve in this.TimelineItems)
            {
                actorClipCurve.Reset();
            }
        }

        public override TimelineItem[] TimelineItems
        {
            get
            {
                return GetComponentsInChildren<CinemaActorClipCurve>();
            }
        }
    }
}