// Cinema Suite

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    public class MultiCurveTrack : ActorTrack
    {

        public override void Initialize()
        {
            foreach (CinemaMultiActorCurveClip clipCurve in this.TimelineItems)
            {
                clipCurve.Initialize();
            }
        }

        public override void UpdateTrack(float time, float deltaTime)
        {
            base.elapsedTime = time;
            foreach (CinemaMultiActorCurveClip clipCurve in this.TimelineItems)
            {
                clipCurve.SampleTime(time);
            }
        }

        public override void SetTime(float time)
        {
            base.elapsedTime = time;
            foreach (CinemaMultiActorCurveClip clipCurve in this.TimelineItems)
            {
                clipCurve.SampleTime(time);
            }
        }

        public override void Stop()
        {
            foreach (CinemaMultiActorCurveClip clipCurve in this.TimelineItems)
            {
                clipCurve.Revert();
            }
        }

        public override TimelineItem[] TimelineItems
        {
            get
            {
                return GetComponentsInChildren<CinemaMultiActorCurveClip>();
            }
        }


    }
}