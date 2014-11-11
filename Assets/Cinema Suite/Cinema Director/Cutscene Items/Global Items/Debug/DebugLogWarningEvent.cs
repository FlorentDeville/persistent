using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Debug", "Log Warning")]
    public class DebugLogWarningEvent : CinemaGlobalEvent
    {

        public string message = "Warning Message";

        public override void Trigger()
        {
            Debug.LogWarning(message);
        }

        public override void Reverse()
        {
            Debug.LogWarning(string.Format("Reverse: {0}", message));
        }
    }
}