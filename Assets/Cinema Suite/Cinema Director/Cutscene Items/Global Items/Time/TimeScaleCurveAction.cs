using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    [CutsceneItem("Time", "Time Scale Curve")]
    public class TimeScaleCurveAction : CinemaGlobalAction
    {
        public AnimationCurve Curve;


        public override void Trigger()
        {

        }

        public override void UpdateTime(float time, float deltaTime)
        {
            Time.timeScale = Curve.Evaluate(time - Firetime);
        }

        public override void End()
        {
        }

    }
}