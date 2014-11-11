using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Time", "Set Time Scale")]
    public class SetTimeScaleEvent : CinemaGlobalEvent
    {
        public float TimeScale = 1f;

        private float savedTimescale = 1f;

        public override void Trigger()
        {
            savedTimescale = Time.timeScale;
            Time.timeScale = TimeScale;
        }

        public override void Reverse()
        {
            Time.timeScale = savedTimescale;
        }
    }
}